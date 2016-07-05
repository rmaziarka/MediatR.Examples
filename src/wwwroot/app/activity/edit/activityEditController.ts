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
        public config: IActivityConfig;
        public activity: ActivityEditModel;
        public userData: Dto.ICurrentUser;

        public enumTypeActivityStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.ActivityStatus;
        private departmentsController: Antares.Attributes.ActivityDepartmentsEditControlController;
        public standardDepartmentType: Dto.IEnumTypeItem;

        public isPropertyPreviewPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;

        public availableAttendeeUsers: Business.User[];
        public availableAttendeeContacts: Business.Contact[];

        private departmentErrorMessageCode: string = 'DEPARTMENTS.COMMON.NEWDEPARTMENTISNOTRELATEDWITHNEGOTIATORERROR.MESSAGE';
        private departmentErrorTitleCode: string = 'DEPARTMENTS.COMMON.NEWDEPARTMENTISNOTRELATEDWITHNEGOTIATORERROR.TITLE';
        private defaultActivityStatusCode: string = 'PreAppraisal';

        //controls
        controlSchemas: any = {           
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

        activityDisposalTypeSchema: Antares.Attributes.IEnumItemEditControlSchema = {
            controlId: 'disposalTypeId',
            translationKey: 'ACTIVITY.COMMON.DISPOSAL_TYPE',
            fieldName: 'disposalTypeId',
            formName: 'disposalTypeForm',
            enumTypeCode: Dto.EnumTypeCode.DisposalType
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
            formName: 'keyNumberForm',
            maxLength: 128
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
            fieldName: 'appraisalMeetingInvitationText',
            formName: 'invitationTextForm'
        }

        activityDecorationSchema: Antares.Attributes.IEnumItemEditControlSchema = {
            controlId: 'decorationId',
            translationKey: 'ACTIVITY.COMMON.DECORATION',
            fieldName: 'decorationId',
            formName: 'decorationForm',
            enumTypeCode: Dto.EnumTypeCode.Decoration
        }

        kfValuationPriceSchema: Antares.Attributes.IPriceEditControlSchema = {
            controlId: 'kfValuationPrice',
            translationKey: 'ACTIVITY.COMMON.KF_VALUTATION',
            formName: 'kfValuationPriceForm',
            fieldName: 'kfValuationPrice'
        }

        vendorValuationPriceSchema: Antares.Attributes.IPriceEditControlSchema = {
            controlId: 'vendorValuationPrice',
            translationKey: 'ACTIVITY.COMMON.VENDOR_VALUATION',
            formName: 'vendorValuationPriceForm',
            fieldName: 'vendorValuationPrice'
        }

        agreedInitialMarketingPriceSchema: Antares.Attributes.IPriceEditControlSchema = {
            controlId: 'agreedInitialMarketingPrice',
            translationKey: 'ACTIVITY.COMMON.AGREED_INITIAL_MARKETING_PRICE',
            formName: 'agreedInitialMarketingPriceForm',
            fieldName: 'agreedInitialMarketingPrice'
        }


        shortKfValuationPriceSchema = {
            controlId: 'shortKfValuationPrice',
            translationKey: 'ACTIVITY.COMMON.SHORT_LET',
            formName: 'shortKfValuationPriceForm',
            fieldName: 'shortKfValuationPrice',
            suffix: 'ACTIVITY.COMMON.SUFFIX'
        }

        shortVendorValuationPriceSchema = {
            controlId: 'shortVendorValuationPrice',
            translationKey: 'ACTIVITY.COMMON.SHORT_LET',
            formName: 'shortVendorValuationPriceForm',
            fieldName: 'shortVendorValuationPrice',
            suffix: 'ACTIVITY.COMMON.SUFFIX'
        }

        shortAgreedInitialMarketingPriceSchema = {
            controlId: 'shortAgreedInitialMarketingPrice',
            translationKey: 'ACTIVITY.COMMON.SHORT_LET',
            formName: 'shortAgreedInitialMarketingPriceForm',
            fieldName: 'shortAgreedInitialMarketingPrice',
            suffix: 'ACTIVITY.COMMON.SUFFIX'
        }

        longKfValuationPriceSchema = {
            controlId: 'longKfValuationPrice',
            translationKey: 'ACTIVITY.COMMON.LONG_LET',
            formName: 'longKfValuationPriceForm',
            fieldName: 'longKfValuationPrice',
            suffix: 'ACTIVITY.COMMON.SUFFIX'
        }

        longVendorValuationPriceSchema = {
            controlId: 'longVendorValuationPrice',
            translationKey: 'ACTIVITY.COMMON.LONG_LET',
            formName: 'longVendorValuationPriceForm',
            fieldName: 'longVendorValuationPrice',
            suffix: 'ACTIVITY.COMMON.SUFFIX'
        }

        longAgreedInitialMarketingPriceSchema = {
            controlId: 'longAgreedInitialMarketingPrice',
            translationKey: 'ACTIVITY.COMMON.LONG_LET',
            formName: 'longAgreedInitialMarketingPriceForm',
            fieldName: 'longAgreedInitialMarketingPrice',
            suffix: 'ACTIVITY.COMMON.SUFFIX'
        }

        serviceChargeAmountSchema: Antares.Attributes.IPriceEditControlSchema = {
            controlId: 'serviceChargeAmount',
            translationKey: 'ACTIVITY.COMMON.SERVICE_CHARGE',
            formName: 'serviceChargeAmountForm',
            fieldName: 'serviceChargeAmount'
        }

        serviceChargeNoteSchema: Antares.Attributes.ITextEditControlSchema = {
            controlId: 'serviceChargeNote',
            translationKey: 'ACTIVITY.COMMON.SERVICE_CHARGE_NOTE',
            fieldName: 'serviceChargeNote',
            formName: 'serviceChargeNoteForm'
        }

        groundRentAmountSchema: Antares.Attributes.IPriceEditControlSchema = {
            controlId: 'groundRentAmount',
            translationKey: 'ACTIVITY.COMMON.GROUND_RENT',
            formName: 'groundRentAmountForm',
            fieldName: 'groundRentAmount'
        }

        groundRentNoteSchema: Antares.Attributes.ITextEditControlSchema = {
            controlId: 'groundRentNote',
            translationKey: 'ACTIVITY.COMMON.GROUND_RENT_NOTE',
            fieldName: 'groundRentNote',
            formName: 'groundRentNoteForm'
        }

        otherConditionSchema: Antares.Attributes.ITextEditControlSchema = {
            controlId: 'otherCondition',
            translationKey: 'ACTIVITY.COMMON.OTHER_CONDITIONS',
            fieldName: 'otherCondition',
            formName: 'otherConditionForm'
        }

        configMocked: any = {
            attendees: {
                attendees: {
                    active: true
                }
            }
        }

        configAppraisalMeetingDateMocked :any = {
            appraisalMeetingDate:  {start:  { active:  true, required:  true  }, end:  { active:  true, required:  true  }  } 
        }

        constructor(
            private dataAccessService: Services.DataAccessService,
            private $state: ng.ui.IStateService,
            private $q: ng.IQService,
            public kfMessageService: Services.KfMessageService,
            private activityConfigUtils: ActivityConfigUtils,
            private configService: Services.ConfigService,
            private activityService: Activity.ActivityService,
            private latestViewsProvider: Providers.LatestViewsProvider,
            private eventAggregator: Core.EventAggregator,
            private enumProvider: Providers.EnumProvider) {

            this.eventAggregator.with(this).subscribe(Common.Component.CloseSidePanelEvent, () => {
                this.isPropertyPreviewPanelVisible = Enums.SidePanelState.Closed;
            });

            this.eventAggregator.with(this).subscribe(Attributes.OpenPropertyPrewiewPanelEvent, (event: Antares.Attributes.OpenPropertyPrewiewPanelEvent) => {
                this.isPropertyPreviewPanelVisible = Enums.SidePanelState.Opened;
            });
        }

        public $onInit = () => {
            this.setStandardDepartmentType();
            this.setDefaultActivityStatus();
            this.setVendorContacts();
            this.setLeadNegotiator();
            this.setDefaultDepartment();

            this.refreshAvailableAttendeeUsers();
            this.refreshAvailableAttendeeContacts();
        }

        public activityTypeChanged = (activityTypeId: string) => {
            this.activity.activityTypeId = activityTypeId;

            this.reloadConfig(this.activity);
        }

        public activityStatusChanged = (id: string) =>{
            this.activity.activityStatusId = id;
            this.reloadConfig(this.activity);
        }

        public reloadConfig = (activity: Activity.ActivityEditModel) => {
            var entity: Commands.ActivityBaseCommand;
            var pageTypeEnum: Enums.PageTypeEnum;

            if (this.isAddMode()) {
                entity = new Commands.ActivityAddCommand(this.activity);
                pageTypeEnum = Enums.PageTypeEnum.Create;
            }
            else {
                entity = new Commands.ActivityEditCommand(this.activity);
                pageTypeEnum = Enums.PageTypeEnum.Update;
            }

            var addEditConfig = this.configService
                .getActivity(pageTypeEnum, this.activity.property.propertyTypeId, activity.activityTypeId, entity);

            var detailsConfig = this.configService
                .getActivity(Enums.PageTypeEnum.Details, this.activity.property.propertyTypeId, activity.activityTypeId, entity);

            this.$q.all([addEditConfig, detailsConfig])
                .then((configs: IActivityConfig[]) => {
                    this.config = this.activityConfigUtils.merge(configs[0], configs[1]);
                });
        }

        public save = () => {
            var valid = this.anyNewDepartmentIsRelatedWithNegotiator();
            if (!valid) {
                this.kfMessageService.showErrorByCode(this.departmentErrorMessageCode, this.departmentErrorTitleCode);
                return;
            }

            if (this.isAddMode()) {
                var addCommand = new Commands.ActivityAddCommand(this.activity);

                this.activityService.addActivity(addCommand).then((activityDto: Dto.IActivity) => {
                    this.latestViewsProvider.addView(<Common.Models.Commands.ICreateLatestViewCommand>{
                        entityId: activityDto.id,
                        entityType: Enums.EntityTypeEnum.Activity
                    });

                    this.$state.go('app.activity-view', { id: activityDto.id });
                });
            }
            else {
                var editCommand = new Commands.ActivityEditCommand(this.activity);

                this.activityService.updateActivity(editCommand).then((activityDto: Dto.IActivity) => {
                    this.$state.go('app.activity-view', { id: activityDto.id });
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
            this.refreshAvailableAttendeeUsers();
            this.addDepartment(user.department);
        }

        public onNegotiatorRemoved = () => {
            this.refreshAvailableAttendeeUsers();
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

        private refreshAvailableAttendeeUsers = () => {
            this.availableAttendeeUsers = this.getAvailableAttendeeUsers();
        }

        private refreshAvailableAttendeeContacts = () => {
            this.availableAttendeeContacts = this.activity.contacts;
        }

        private getAvailableAttendeeUsers = (): Business.User[] => {

            var users: Business.User[] = [];

            users = this.activity.secondaryNegotiator.map((n: Business.ActivityUser) => {
                return n.user;
            }) || [];

            if (this.activity.leadNegotiator) {
                users.push(this.activity.leadNegotiator.user);
            }

            return users;
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
            if (this.pageMode === PageMode.Edit) {
                return;
            }

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

            var vendor: Business.Ownership = _.find(this.activity.property.ownerships, (ownership: Business.Ownership) => {
                return ownership.isVendor();
            });

            if (vendor) {
                this.activity.contacts = vendor.contacts;
            }
        }

        private setLeadNegotiator = () => {
            if (this.pageMode === PageMode.Edit) {
                return;
            }

            this.activity.leadNegotiator.userId = this.userData.id;
            this.activity.leadNegotiator.user.id = this.userData.id;
            this.activity.leadNegotiator.user.firstName = this.userData.firstName;
            this.activity.leadNegotiator.user.lastName = this.userData.lastName;
            this.activity.leadNegotiator.user.departmentId = this.userData.department.id;

            this.activity.leadNegotiator.callDate = moment().add(2, 'week').toDate();
        }

        private setDefaultDepartment = () => {
            if (this.pageMode === PageMode.Edit) {
                return;
            }

            var defaultDepartment = new Business.ActivityDepartment();
            defaultDepartment.departmentId = this.userData.department.id;
            defaultDepartment.department.id = this.userData.department.id;
            defaultDepartment.department.name = this.userData.department.name;

            var managingTypeEnumItems: Dto.IEnumItem[] = this.enumProvider.enums.activityDepartmentType.filter((enumItem: Dto.IEnumItem) => { return enumItem.code === Enums.DepartmentTypeEnum[Enums.DepartmentTypeEnum.Managing]; });
            defaultDepartment.departmentTypeId = managingTypeEnumItems[0].id;
            defaultDepartment.departmentType.id = managingTypeEnumItems[0].id;
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