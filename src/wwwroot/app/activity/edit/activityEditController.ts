/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;
    import Commands = Common.Models.Commands;

    enum PageMode {
        Add,
        Edit
    }

    export class ActivityEditController {
        public config: IActivityEditViewConfig;
        public activity: Business.ActivityEditModel;
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
        private weeklyCode: string = 'Weekly';
        private monthlyhCode: string = 'Monthly';

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


        shortKfValuationPriceSchema: Antares.Attributes.IPriceEditControlSchema = {
            controlId: 'shortKfValuationPrice',
            translationKey: 'ACTIVITY.COMMON.SHORT_LET',
            formName: 'shortKfValuationPriceForm',
            fieldName: 'shortKfValuationPrice',
            suffix: 'ACTIVITY.COMMON.GBP_PER_WEEK'
        }

        shortVendorValuationPriceSchema: Antares.Attributes.IPriceEditControlSchema = {
            controlId: 'shortVendorValuationPrice',
            translationKey: 'ACTIVITY.COMMON.SHORT_LET',
            formName: 'shortVendorValuationPriceForm',
            fieldName: 'shortVendorValuationPrice',
            suffix: 'ACTIVITY.COMMON.GBP_PER_WEEK'
        }

        shortAgreedInitialMarketingPriceSchema: Antares.Attributes.IPriceEditControlSchema = {
            controlId: 'shortAgreedInitialMarketingPrice',
            translationKey: 'ACTIVITY.COMMON.SHORT_LET',
            formName: 'shortAgreedInitialMarketingPriceForm',
            fieldName: 'shortAgreedInitialMarketingPrice',
            suffix: 'ACTIVITY.COMMON.GBP_PER_WEEK'
        }

        longKfValuationPriceSchema: Antares.Attributes.IPriceEditControlSchema = {
            controlId: 'longKfValuationPrice',
            translationKey: 'ACTIVITY.COMMON.LONG_LET',
            formName: 'longKfValuationPriceForm',
            fieldName: 'longKfValuationPrice',
            suffix: 'ACTIVITY.COMMON.GBP_PER_WEEK'
        }

        longVendorValuationPriceSchema: Antares.Attributes.IPriceEditControlSchema = {
            controlId: 'longVendorValuationPrice',
            translationKey: 'ACTIVITY.COMMON.LONG_LET',
            formName: 'longVendorValuationPriceForm',
            fieldName: 'longVendorValuationPrice',
            suffix: 'ACTIVITY.COMMON.GBP_PER_WEEK'
        }

        longAgreedInitialMarketingPriceSchema: Antares.Attributes.IPriceEditControlSchema = {
            controlId: 'longAgreedInitialMarketingPrice',
            translationKey: 'ACTIVITY.COMMON.LONG_LET',
            formName: 'longAgreedInitialMarketingPriceForm',
            fieldName: 'longAgreedInitialMarketingPrice',
            suffix: 'ACTIVITY.COMMON.GBP_PER_WEEK'
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

        priceTypeSchema: Antares.Attributes.IEnumItemEditControlSchema = {
            controlId: 'priceTypeId',
            translationKey: 'ACTIVITY.COMMON.PRICE_TYPE',
            fieldName: 'priceTypeId',
            formName: 'priceTypeForm',
            enumTypeCode: Dto.EnumTypeCode.ActivityPriceType
        }

        activityPriceSchema: Antares.Attributes.IPriceEditControlSchema = {
            controlId: 'activityPrice',
            translationKey: 'ACTIVITY.COMMON.PRICE',
            formName: 'activityPricetForm',
            fieldName: 'activityPrice'
        }

        matchFlexibilitySchema: Antares.Attributes.IEnumItemEditControlSchema = {
            controlId: 'matchFlexibilityId',
            translationKey: '',
            fieldName: 'matchFlexibilityId',
            formName: 'matchFlexibilityForm',
            enumTypeCode: Dto.EnumTypeCode.ActivityMatchFlexPrice
        }

        matchFlexValueSchema: Antares.Attributes.IPriceEditControlSchema = {
            controlId: 'matchFlexValue',
            translationKey: '',
            formName: 'matchFlexValueForm',
            fieldName: 'matchFlexValue'
        }

        matchFlexPercentageSchema: Antares.Attributes.IPriceEditControlSchema = {
            controlId: 'matchFlexPercentage',
            translationKey: '',
            formName: 'matchFlexPercentageForm',
            fieldName: 'matchFlexPercentage',
            suffix: 'ACTIVITY.COMMON.PERCENT'
        }

        rentPaymentPeriodSchema: Antares.Attributes.IEnumItemEditControlSchema = {
            controlId: 'rentPaymentPeriodId',
            translationKey: 'ACTIVITY.COMMON.RENT',
            fieldName: 'rentPaymentPeriodId',
            formName: 'rentPaymentPeriodForm',
            enumTypeCode: Dto.EnumTypeCode.RentPaymentPeriod
        }

        shortAskingWeekRentSchema: Antares.Attributes.IPriceEditControlSchema = {
            controlId: 'shortAskingWeekRent',
            translationKey: '',
            formName: 'shortAskingWeekRentForm',
            fieldName: 'shortAskingWeekRent'
        }

        shortAskingMonthRentSchema: Antares.Attributes.IPriceEditControlSchema = {
            controlId: 'shortAskingMonthRent',
            translationKey: '',
            formName: 'shortAskingMonthRentForm',
            fieldName: 'shortAskingMonthRent'
        }

        shortMatchFlexibilitySchema: Antares.Attributes.IEnumItemEditControlSchema = {
            controlId: 'shortMatchFlexibilityId',
            translationKey: '',
            fieldName: 'shortMatchFlexibilityId',
            formName: 'shortMatchFlexibilityForm',
            enumTypeCode: Dto.EnumTypeCode.ActivityMatchFlexRent
        }

        shortMatchFlexWeekValueSchema: Antares.Attributes.IPriceEditControlSchema = {
            controlId: 'shortMatchFlexWeekValue',
            translationKey: '',
            formName: 'shortMatchFlexWeekValueForm',
            fieldName: 'shortMatchFlexWeekValue'
        }

        shortMatchFlexMonthValueSchema: Antares.Attributes.IPriceEditControlSchema = {
            controlId: 'shortMatchFlexMonthValue',
            translationKey: '',
            formName: 'shortMatchFlexMonthValueForm',
            fieldName: 'shortMatchFlexMonthValue',
        }

        shortMatchFlexPercentageSchema: Antares.Attributes.IPriceEditControlSchema = {
            controlId: 'shortMatchFlexPercentage',
            translationKey: '',
            formName: 'shortMatchFlexPercentageForm',
            fieldName: 'shortMatchFlexPercentage',
            suffix: 'ACTIVITY.COMMON.PERCENT'
        }

        longAskingWeekRentSchema: Antares.Attributes.IPriceEditControlSchema = {
            controlId: 'longAskingWeekRent',
            translationKey: '',
            formName: 'longAskingWeekRentForm',
            fieldName: 'longAskingWeekRent'
        }

        longAskingMonthRentSchema: Antares.Attributes.IPriceEditControlSchema = {
            controlId: 'longAskingMonthRent',
            translationKey: '',
            formName: 'longAskingMonthRentForm',
            fieldName: 'longAskingMonthRent'
        }

        longMatchFlexibilitySchema: Antares.Attributes.IEnumItemEditControlSchema = {
            controlId: 'longMatchFlexibilityId',
            translationKey: '',
            fieldName: 'longMatchFlexibilityId',
            formName: 'longMatchFlexibilityForm',
            enumTypeCode: Dto.EnumTypeCode.ActivityMatchFlexRent
        }

        longMatchFlexWeekValueSchema: Antares.Attributes.IPriceEditControlSchema = {
            controlId: 'longMatchFlexWeekValue',
            translationKey: '',
            formName: 'longMatchFlexWeekValueForm',
            fieldName: 'longMatchFlexWeekValue'
        }

        longMatchFlexMonthValueSchema: Antares.Attributes.IPriceEditControlSchema = {
            controlId: 'longMatchFlexMonthValue',
            translationKey: '',
            formName: 'longMatchFlexMonthValueForm',
            fieldName: 'longMatchFlexMonthValue'
        }

        longMatchFlexPercentageSchema: Antares.Attributes.IPriceEditControlSchema = {
            controlId: 'longMatchFlexPercentage',
            translationKey: '',
            formName: 'longMatchFlexPercentageForm',
            fieldName: 'longMatchFlexPercentage',
            suffix: 'ACTIVITY.COMMON.PERCENT'
        }
        
        constructor(
            private dataAccessService: Services.DataAccessService,
            private $state: ng.ui.IStateService,
            private $q: ng.IQService,
            public kfMessageService: Services.KfMessageService,
            private configService: Services.ConfigService,
            private activityService: Services.ActivityService,
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
            if (this.config != null && this.config.editConfig != null) {
                this.config.editConfig.activityStatus.activityStatusId.active = false;
            }

            this.reloadConfig(this.activity);
        }

        public activityStatusChanged = (id: string) => {
            this.activity.activityStatusId = id;
            this.reloadConfig(this.activity);
        }

        public matchFlexibilityChanged = (id: string) => {
            this.activity.matchFlexibilityId = id;
            this.reloadConfig(this.activity);
        }

        public rentPaymentPeriodChanged = (id: string) => {
            this.activity.rentPaymentPeriodId = id;
            this.copyRentValues();
            this.reloadConfig(this.activity);
        }

        public shortMatchFlexibilityChanged = (id: string) => {
            this.activity.shortMatchFlexibilityId = id;
            this.reloadConfig(this.activity);
        }

        public longMatchFlexibilityChanged = (id: string) => {
            this.activity.longMatchFlexibilityId = id;
            this.reloadConfig(this.activity);
        }

        public reloadConfig = (activity: Business.ActivityEditModel) => {
            var entity: Commands.Activity.ActivityBaseCommand;
            var pageTypeEnum: Enums.PageTypeEnum;

            if (this.isAddMode()) {
                entity = new Commands.Activity.ActivityAddCommand(this.activity);
                pageTypeEnum = Enums.PageTypeEnum.Create;
            }
            else {
                entity = new Commands.Activity.ActivityEditCommand(this.activity);
                pageTypeEnum = Enums.PageTypeEnum.Update;
            }

            var addEditConfig = this.configService
                .getActivity(pageTypeEnum, this.activity.property.propertyTypeId, activity.activityTypeId, entity);

            var detailsConfig = this.configService
                .getActivity(Enums.PageTypeEnum.Details, this.activity.property.propertyTypeId, activity.activityTypeId, entity);

            this.$q.all([addEditConfig, detailsConfig])
                .then((configs: IActivityConfig[]) => {
                    this.config = <IActivityEditViewConfig>{
                        editConfig: configs[0],
                        viewConfig: configs[1]
                    };
                });
        }

        public save = () => {
            var valid = this.anyNewDepartmentIsRelatedWithNegotiator();
            if (!valid) {
                this.kfMessageService.showErrorByCode(this.departmentErrorMessageCode, this.departmentErrorTitleCode);
                return;
            }

            if (this.activity.rentPaymentPeriodId != null) {

                this.calculateRentPayments();
            }

            if (this.isAddMode()) {
                var addCommand = new Commands.Activity.ActivityAddCommand(this.activity);

                this.activityService.addActivity(addCommand).then((activityDto: Dto.IActivity) => {
                    this.latestViewsProvider.addView(<Common.Models.Commands.ICreateLatestViewCommand>{
                        entityId: activityDto.id,
                        entityType: Enums.EntityTypeEnum.Activity
                    });

                    this.$state.go('app.activity-view', { id: activityDto.id });
                });
            }
            else {
                var editCommand = new Commands.Activity.ActivityEditCommand(this.activity);

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

        public isOtherSectionVisible = (): Boolean => {
            return this.config && this.config.editConfig && this.config.editConfig.decoration != null && this.config.editConfig.otherCondition != null;
        }

        public isValuationInfoSectionVisible = (): Boolean => {
            return this.config && this.config.editConfig && this.config.editConfig.kfValuationPrice != null && this.config.editConfig.vendorValuationPrice != null &&
                this.config.editConfig.agreedInitialMarketingPrice != null;
        }

        public isValuationInfoShortLongSectionVisible = (): Boolean => {
            return this.config && this.config.editConfig && this.config.editConfig.shortKfValuationPrice != null && this.config.editConfig.longKfValuationPrice != null &&
                this.config.editConfig.shortVendorValuationPrice != null && this.config.editConfig.longVendorValuationPrice != null &&
                this.config.editConfig.shortAgreedInitialMarketingPrice != null && this.config.editConfig.longAgreedInitialMarketingPrice != null;
        }

        public isChargesSectionVisible = (): Boolean => {
            return this.config && this.config.editConfig && this.config.editConfig.serviceChargeAmount != null && this.config.editConfig.groundRentAmount != null && this.config.editConfig.groundRentNote != null;
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

            this.activity.leadNegotiator.callDate = moment().add(2, 'week').startOf('day').toDate();
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

        public isBasicInformationSectionVisible = (): Boolean => {
            return this.config && this.config.editConfig && (this.config.editConfig.property != null || this.config.editConfig.disposalType != null || this.config.editConfig.source != null ||
                this.config.editConfig.sourceDescription != null || this.config.editConfig.sellingReason != null || this.config.editConfig.pitchingThreats != null);
        }

        public isAdditionalInformationSectionVisible = (): Boolean => {
            return this.config && this.config.editConfig && (this.config.editConfig.keyNumber != null || this.config.editConfig.accessArrangements != null);
        }

        public isAppraisalMeetingSectionVisible = (): Boolean => {
            return this.config && this.config.editConfig && (this.config.editConfig.appraisalMeetingDate != null ||
                this.config.editConfig.appraisalMeetingAttendees != null || this.config.editConfig.appraisalMeetingInvitation != null);
        }

        public copyRentValues = () => {
            if (this.activity.rentPaymentPeriodId === this.getRentPaymentPeriodId(this.weeklyCode)) {
                this.copyRentMonthValuesToWeekValues();
            }

            if (this.activity.rentPaymentPeriodId === this.getRentPaymentPeriodId(this.monthlyhCode)) {
                this.copyRentWeekValuesToMonthValues();
            }
        }

        private copyRentMonthValuesToWeekValues = () => {
            this.activity.shortAskingWeekRent = this.activity.shortAskingMonthRent;
            this.activity.shortMatchFlexWeekValue = this.activity.shortMatchFlexMonthValue;
            this.activity.longAskingWeekRent = this.activity.longAskingMonthRent;
            this.activity.longMatchFlexWeekValue = this.activity.longMatchFlexMonthValue;
        }

        private copyRentWeekValuesToMonthValues = () => {
            this.activity.shortAskingMonthRent = this.activity.shortAskingWeekRent;
            this.activity.shortMatchFlexMonthValue = this.activity.shortMatchFlexWeekValue;
            this.activity.longAskingMonthRent = this.activity.longAskingWeekRent;
            this.activity.longMatchFlexMonthValue = this.activity.longMatchFlexWeekValue;
        }


        public calculateRentPayments = () => {
            if (this.activity.rentPaymentPeriodId === this.getRentPaymentPeriodId(this.weeklyCode)) {
                this.updateMonthValues();
            }

            if (this.activity.rentPaymentPeriodId === this.getRentPaymentPeriodId(this.monthlyhCode)) {
                this.updateWeekValues();
            }
        }

        private updateWeekValues = () => {
            this.activity.shortAskingWeekRent = this.convertPerMonthValueToPerWeekValue(this.activity.shortAskingMonthRent);
            this.activity.shortMatchFlexWeekValue = this.convertPerMonthValueToPerWeekValue(this.activity.shortMatchFlexMonthValue);
            this.activity.longAskingWeekRent = this.convertPerMonthValueToPerWeekValue(this.activity.longAskingMonthRent);
            this.activity.longMatchFlexWeekValue = this.convertPerMonthValueToPerWeekValue(this.activity.longMatchFlexMonthValue);
        }

        private updateMonthValues = () => {
            this.activity.shortAskingMonthRent = this.convertPerWeekValueToPerMonthValue(this.activity.shortAskingWeekRent);
            this.activity.shortMatchFlexMonthValue = this.convertPerWeekValueToPerMonthValue(this.activity.shortMatchFlexWeekValue);
            this.activity.longAskingMonthRent = this.convertPerWeekValueToPerMonthValue(this.activity.longAskingWeekRent);
            this.activity.longMatchFlexMonthValue = this.convertPerWeekValueToPerMonthValue(this.activity.longMatchFlexWeekValue);
        }

        private getRentPaymentPeriodId = (rentPaymentPeriodCode: string): string => {
            var selectedRentPaymentPeriod: any = <Dto.IEnumTypeItem>_.find(this.enumProvider.enums[Dto.EnumTypeCode.RentPaymentPeriod], (item: Dto.IEnumTypeItem) => {
                return item.code === rentPaymentPeriodCode;
            });

            return selectedRentPaymentPeriod.id;
        }

        public convertPerWeekValueToPerMonthValue(weekValue: number): number {
            if (weekValue == null) {
                return null;
            }
            else {
                return Math.ceil((weekValue * 52.0) / 12.0);
            }
        }

        public convertPerMonthValueToPerWeekValue(monthValue: number): number {
            if (monthValue == null) {
                return null;
            }
            else {
                return Math.ceil((monthValue * 12.0) / 52.0);
            }
        }
    }

    angular.module('app').controller('ActivityEditController', ActivityEditController);
};