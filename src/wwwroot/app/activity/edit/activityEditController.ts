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
        public property: Business.Property;
        public enumTypeActivityStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.ActivityStatus;
        private departmentsController: Antares.Attributes.ActivityDepartmentsEditControlController;

        public standardDepartmentType: Dto.IEnumTypeItem;

        public isPropertyPreviewPanelVisible: boolean = false;

        private departmentErrorMessageCode: string = 'DEPARTMENTS.COMMON.NEWDEPARTMENTISNOTRELATEDWITHNEGOTIATORERROR.MESSAGE';
        private departmentErrorTitleCode: string = 'DEPARTMENTS.COMMON.NEWDEPARTMENTISNOTRELATEDWITHNEGOTIATORERROR.TITLE';

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

        constructor(
            private dataAccessService: Services.DataAccessService,
            private $state: ng.ui.IStateService,
            private enumService: Services.EnumService,
            public kfMessageService: Services.KfMessageService,
            private eventAggregator: Core.EventAggregator) {

            this.enumService.getEnumPromise().then(this.onEnumLoaded);


            this.eventAggregator.with(this).subscribe(Common.Component.CloseSidePanelEvent, () => {
                this.isPropertyPreviewPanelVisible = false;
            });

            this.eventAggregator.with(this).subscribe(Attributes.OpenPropertyPrewiewPanelEvent, (event: Antares.Attributes.OpenPropertyPrewiewPanelEvent) => {
                this.isPropertyPreviewPanelVisible = true;
            });
        }
        
        $onInit = () => {
            if (this.pageMode === PageMode.Add && !this.property) {
                this.$state.go('app.default');
            }
        }
        
        public save() {
            var valid = this.anyNewDepartmentIsRelatedWithNegotiator();
            if (!valid) {
                this.kfMessageService.showErrorByCode(this.departmentErrorMessageCode, this.departmentErrorTitleCode);
                return;
            }

            this.dataAccessService.getActivityResource()
                .update(new Business.UpdateActivityResource(this.activity))
                .$promise
                .then((activity: Dto.IActivity) => {
                    this.$state.go('app.activity-view', { id: activity.id });
                });
        }

        activityStatusChanged = (activityStatusId: string) => {
        }

        cancel() {
            this.$state.go('app.activity-view', { id: this.activity.id });
        }

        setDepartmentsEdit(departmentsController: Antares.Attributes.ActivityDepartmentsEditControlController) {
            this.departmentsController = departmentsController;
        }

        public onNegotiatorAdded = (user: Dto.IUser) => {
            this.addDepartment(user.department);
        }

        public departmentIsRelatedWithNegotiator = (department: Business.Department) => {
            return this.activity.leadNegotiator.user.departmentId === department.id ||
                _.some(this.activity.secondaryNegotiator, (item) => item.user.departmentId === department.id);
        } 

        private get pageMode(): PageMode {
            return this.activity && this.activity.id ? PageMode.Edit : PageMode.Add;
        }

        private onEnumLoaded = (result: any) => {
            var departmentTypes: any = result[Dto.EnumTypeCode.ActivityDepartmentType];
            this.standardDepartmentType = <Dto.IEnumTypeItem>_.find(departmentTypes, (item: Dto.IEnumTypeItem) => {
                return item.code === Enums.DepartmentTypeEnum[Enums.DepartmentTypeEnum.Standard];
            });
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