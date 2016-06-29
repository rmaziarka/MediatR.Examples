/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;

    enum PageMode {
        Add,
        Edit
    }

    export class ActivityEditController {
        public config: IActivityEditConfig;
        public activity: Business.Activity;
        public property: Business.PropertyView;
        public userData: Dto.IUserData;

        public propertyDivisionId: string;
        public propertyTypeId: string;
        public enumTypeActivityStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.ActivityStatus;
        private departmentsController: Antares.Attributes.ActivityDepartmentsEditControlController;
        public standardDepartmentType: Dto.IEnumTypeItem;

        public isPropertyPreviewPanelVisible: boolean = false;

        public availableAttendeeUsers: Business.User[];
        public availableAttendeeContacts: Business.Contact[];

        private departmentErrorMessageCode: string = 'DEPARTMENTS.COMMON.NEWDEPARTMENTISNOTRELATEDWITHNEGOTIATORERROR.MESSAGE';
        private departmentErrorTitleCode: string = 'DEPARTMENTS.COMMON.NEWDEPARTMENTISNOTRELATEDWITHNEGOTIATORERROR.TITLE';
        private defaultActivityStatusCode: string = 'PreAppraisal';

        //controls
        controlSchemas: any = {
            marketAppraisalPrice: {
                formName: "marketAppraisalPriceControlForm",
                controlId: "market-appraisal-price",
                translationKey: "ACTIVITY.EDIT.PRICES.MARKET_APPRAISAL_PRICE",
                fieldName: "marketAppraisalPrice"
            },
            recommendedPrice: {
                formName: "recommendedPriceControlForm",
                controlId: "recommended-price",
                translationKey: "ACTIVITY.EDIT.PRICES.RECOMMENDED_PRICE",
                fieldName: "recommendedPrice"
            },
            vendorEstimatedPrice: {
                formName: "vendorEstimatedPriceControlForm",
                controlId: "vendor-estimated-price",
                translationKey: "ACTIVITY.EDIT.PRICES.VENDOR_ESTIMATED_PRICE",
                fieldName: "vendorEstimatedPrice"
            },
            askingPrice: {
                formName: "askingPriceControlForm",
                controlId: "asking-price",
                translationKey: "ACTIVITY.EDIT.PRICES.ASKING_PRICE",
                fieldName: "askingPrice"
            },
            shortLetPricePerWeek: {
                formName: "shortLetPricePerWeekControlForm",
                controlId: "short-let-price-per-week",
                translationKey: "ACTIVITY.EDIT.PRICES.SHORT_LET_PRICE_PER_WEEK",
                fieldName: "shortLetPricePerWeek"
            }
        };

        activitySourceSchema: Antares.Attributes.IEnumItemEditControlSchema = {
            controlId: 'sourceId',
            translationKey: 'ACTIVITY.EDIT.SOURCE',
            fieldName: 'sourceId',
            formName: 'sourceForm',
            enumTypeCode: Dto.EnumTypeCode.ActivitySource
        }

        activityStatusSchema: Antares.Attributes.IEnumItemEditControlSchema = {
            controlId: 'activityStatusId',
            translationKey: 'ACTIVITY.EDIT.STATUS',
            fieldName: 'activityStatusId',
            formName: 'activityStatusForm',
            enumTypeCode: Dto.EnumTypeCode.ActivityStatus
        }

        activitySellingReasonSchema: Antares.Attributes.IEnumItemEditControlSchema = {
            controlId: 'sellingReasonId',
            translationKey: 'ACTIVITY.EDIT.SELLING_REASON',
            fieldName: 'sellingReasonId',
            formName: 'sellingReasonForm',
            enumTypeCode: Dto.EnumTypeCode.ActivitySellingReason
        }

        activitySourceDescriptionSchema: Antares.Attributes.ITextEditControlSchema = {
            controlId: 'sourceDescriptionId',
            translationKey: 'ACTIVITY.EDIT.SOURCE_DESCRIPTION',
            fieldName: 'sourceDescription',
            formName: 'sourceDescriptionForm'
        }

        keyNumberSchema: Antares.Attributes.ITextEditControlSchema = {
            controlId: 'keyNumberId',
            translationKey: 'ACTIVITY.EDIT.KEY_NUMBER',
            fieldName: 'keyNumber',
            formName: 'keyNumberForm'
        }

        pitchingThreatsSchema: Antares.Attributes.ITextEditControlSchema = {
            controlId: 'pitchingThreatsId',
            translationKey: 'ACTIVITY.EDIT.PITCHING_THREATS',
            fieldName: 'pitchingThreats',
            formName: 'pitchingThreatsForm'
        }

        accessArrangementsSchema: Antares.Attributes.ITextEditControlSchema = {
            controlId: 'accessArrangementsId',
            translationKey: 'ACTIVITY.EDIT.ACCESS_ARRANGEMENTS',
            fieldName: 'accessArrangements',
            formName: 'accessArrangementsForm'
        }
   
        invitationTextSchema: Antares.Attributes.ITextEditControlSchema = {
            controlId: 'invitationTextId',
            translationKey: 'ACTIVITY.EDIT.APPRAISAL_MEETING.INVITATION_TEXT',
            fieldName: 'invitationText',
            formName: 'invitationTextForm'
        }

        configMocked: any = {
            attendees: {
                attendees: {
                    active: true
                }
            }
        }

        constructor(
            private dataAccessService: Services.DataAccessService,
            private $state: ng.ui.IStateService,
            public kfMessageService: Services.KfMessageService,
            private configService: Services.ConfigService,
            private activityService: Activity.ActivityService,
            private latestViewsProvider: Providers.LatestViewsProvider,
            private eventAggregator: Core.EventAggregator,
            private enumProvider: Providers.EnumProvider) {

            this.eventAggregator.with(this).subscribe(Common.Component.CloseSidePanelEvent, () => {
                this.isPropertyPreviewPanelVisible = false;
            });

            this.eventAggregator.with(this).subscribe(Attributes.OpenPropertyPrewiewPanelEvent, (event: Antares.Attributes.OpenPropertyPrewiewPanelEvent) => {
                this.isPropertyPreviewPanelVisible = true;
            });
        }

        public $onInit = () => {
            this.activity = this.activity || new Business.Activity();
            this.propertyTypeId = this.isAddMode() ? this.property.propertyTypeId : this.activity.property.propertyTypeId;
            this.propertyDivisionId = this.isAddMode() ? this.property.division.id : this.activity.property.divisionId;

            this.setStandardDepartmentType();
            this.setDefaultActivityStatus();
            this.setVendorContacts();
            this.setLeadNegotiator();
            this.setDefaultDepartment();

            this.availableAttendeeUsers = this.getAvailableAttendeeUsers();
            this.availableAttendeeContacts = this.getAvailableAttendeeContacts();
        }

        public activityTypeChanged = (activityTypeId: string) => {
            this.activity.activityTypeId = activityTypeId;

            this.reloadConfig(this.activity);
        }

        public activityStatusChanged = (activityStatusId: string) => {
            this.reloadConfig(this.activity);
        }

        public reloadConfig = (activity: Business.Activity) => {
            var entity = new Business.UpdateActivityResource(this.activity);

            this.configService
                .getActivity(Enums.PageTypeEnum.Create, this.propertyTypeId, activity.activityTypeId, entity)
                .then((newConfig: IActivityEditConfig) => this.config = newConfig);
        }

        public save = () => {
            var valid = this.anyNewDepartmentIsRelatedWithNegotiator();
            if (!valid) {
                this.kfMessageService.showErrorByCode(this.departmentErrorMessageCode, this.departmentErrorTitleCode);
                return;
            }

            if (this.isAddMode()) {
                var addCommand = new Commands.ActivityAddCommand(this.activity, this.property.id);

                this.activityService.addActivity(addCommand).then((activityDto: Dto.IActivity) => {
                    this.latestViewsProvider.addView(<Common.Models.Commands.ICreateLatestViewCommand>{
                        entityId: activityDto.id,
                        entityType: Enums.EntityTypeEnum.Activity
                    });

                    this.$state.go('app.activity-view', { id: this.activity.id });
                });
            }
            else {
                var editCommand = new Commands.ActivityEditCommand(this.activity);

                this.activityService.updateActivity(editCommand).then((activityDto: Dto.IActivity) => {
                    //this.latestViewsProvider.addView(<Common.Models.Commands.ICreateLatestViewCommand>{
                    //    entityId: activityDto.id,
                    //    entityType: Enums.EntityTypeEnum.Activity
                    //});
                    this.$state.go('app.activity-view', { id: this.activity.id });
                });
            }
        }

        public cancel() {
            this.$state.go('app.activity-view', { id: this.activity.id });
        }

        public setDepartmentsEdit(departmentsController: Antares.Attributes.ActivityDepartmentsEditControlController) {
            this.departmentsController = departmentsController;
        }

        public onNegotiatorAdded = (user: Dto.IUser) => {
            this.availableAttendeeUsers = this.getAvailableAttendeeUsers();
            this.addDepartment(user.department);
        }

        public departmentIsRelatedWithNegotiator = (department: Business.Department) => {
            return this.activity.leadNegotiator.user.departmentId === department.id ||
                _.some(this.activity.secondaryNegotiator, (item) => item.user.departmentId === department.id);
        }

        public isAddMode = () => {
            return this.pageMode === PageMode.Add;
        }

        public isEditMode = () => {
            return this.pageMode === PageMode.Edit;
        }

        public getAvailableAttendeeUsers = (): Business.User[] => {

            var users: Business.User[] = [];
            
            this.activity.secondaryNegotiator.map((n: Business.ActivityUser) => {
                return n.user;
            }) || [];

            if (this.activity.leadNegotiator) {
                users.push(this.activity.leadNegotiator.user);
            }

            return users;
        }

        public getAvailableAttendeeContacts = (): Business.Contact[] => {
            return this.activity.contacts;
        }

        private get pageMode(): PageMode {
            return this.activity && this.activity.id ? PageMode.Edit : PageMode.Add;
        }

        private setStandardDepartmentType = () => {
            var departmentTypes: any = this.enumProvider.enums[Dto.EnumTypeCode.ActivityDepartmentType];
            this.standardDepartmentType = <Dto.IEnumTypeItem>_.find(departmentTypes, (item: Dto.IEnumTypeItem) => {
                return item.code === Enums.DepartmentTypeEnum[Enums.DepartmentTypeEnum.Standard];
            });
        }

        private setDefaultActivityStatus = () => {
            var activityStatuses = this.enumProvider.enums[Dto.EnumTypeCode.ActivityStatus];
            var defaultActivityStatus: any = <Dto.IEnumTypeItem>_.find(activityStatuses, (item: Dto.IEnumTypeItem) => {
                return item.code === this.defaultActivityStatusCode;
            });

            if (defaultActivityStatus) {
                this.activity.activityStatusId = defaultActivityStatus.id;
            }
        }

        private setVendorContacts = (): void => {
            if (this.pageMode === PageMode.Edit) {
                return;
            }

            var vendor: Business.Ownership = _.find(this.property.ownerships, (ownership: Business.Ownership) => {
                return ownership.isVendor();
            });

            if (vendor) {
                this.activity.contacts = vendor.contacts;
            }
        }

        private setLeadNegotiator = () =>{
            if (this.pageMode === PageMode.Edit) {
                return;
            }

            this.activity.leadNegotiator.user.id = this.userData.id;
            this.activity.leadNegotiator.user.firstName = this.userData.firstName;
            this.activity.leadNegotiator.user.lastName = this.userData.lastName;
            this.activity.leadNegotiator.user.departmentId = this.userData.department.id;
        }

        private setDefaultDepartment = () =>{
            if (this.pageMode === PageMode.Edit) {
                return;
            }

            var defaultDepartment = new Business.ActivityDepartment();
            defaultDepartment.departmentId = this.userData.department.id;
            defaultDepartment.department.id = this.userData.department.id;
            defaultDepartment.department.name = this.userData.department.name;
            defaultDepartment.departmentType.code = Enums.DepartmentTypeEnum[Enums.DepartmentTypeEnum.Managing];
            this.activity.activityDepartments.push(defaultDepartment);
        }

        private anyNewDepartmentIsRelatedWithNegotiator = () => {
            var newDepartments = this.activity.activityDepartments.filter((item: Business.ActivityDepartment) => { return !item.id });
            return _.all(newDepartments, (item) => this.departmentIsRelatedWithNegotiator(item.department));
        }

        private addDepartment(department: Business.Department) {
            if (!_.some(this.activity.activityDepartments, (activityDepartment: Business.ActivityDepartment) => { return activityDepartment.departmentId === department.id })) {
                this.activity.activityDepartments.push(this.createActivityDepartment(department));
            }
        }

        private createActivityDepartment = (department: Dto.IDepartment) => {
            var activityDepartment = new Business.ActivityDepartment();
            activityDepartment.activityId = this.activity.id;
            activityDepartment.departmentId = department.id;
            activityDepartment.department = new Business.Department(<Dto.IDepartment>department);
            activityDepartment.departmentType = this.standardDepartmentType;
            activityDepartment.departmentTypeId = this.standardDepartmentType.id;

            return activityDepartment;
        }
    }

    angular.module('app').controller('ActivityEditController', ActivityEditController);
};