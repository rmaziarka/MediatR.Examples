/// <reference path="../../../../typings/_all.d.ts" />

module Antares {
    import Business = Common.Models.Business;
    interface IScope extends ng.IScope {
        leadNegotiator: Business.ActivityUser;
        secondaryNegotiator: Business.ActivityUser[];
    }

    describe('Given negotiators component', () => {
        var pageObjectSelector = {
            secondaryNegotiatorItems: '*[id^="SecondaryNegotiator"]',
            leadNegotiatorItem: '*[id^="LeadNegotiator"]',
            noSecondaryNegotiators: '*[id="empty-secondary-negotiators"]'
        };
        var assertValidator: TestHelpers.AssertValidators;
        var scope: IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery;

        var mockedComponentHtml = '<negotiators lead-negotiator="leadNegotiator" secondary-negotiators="secondaryNegotiator">'
            + '</negotiators>';
        
        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {

            // init
            scope = <IScope>$rootScope.$new();
            scope.leadNegotiator = new Business.ActivityUser();
            scope.secondaryNegotiator = [];

            compile = $compile;
            element = compile(mockedComponentHtml)(scope);
            scope.$apply();

            assertValidator = new TestHelpers.AssertValidators(element, scope);
        }));

        describe('when activity is edited', () => {
            var leadNegotiator = TestHelpers.ActivityUserGenerator.generate(Common.Models.Enums.NegotiatorTypeEnum.LeadNegotiator);
            var secondaryNegotiators = TestHelpers.ActivityUserGenerator.generateMany(3, Common.Models.Enums.NegotiatorTypeEnum.SecondaryNegotiator);
            
            beforeEach(() => {               
               scope.leadNegotiator = leadNegotiator;
               scope.secondaryNegotiator = secondaryNegotiators;               
               scope.$apply();
            });
            
            it('then it should display single lead negotiator entity', () => {                
                var leadNegotiatorUserData: ng.IAugmentedJQuery = element.find(pageObjectSelector.leadNegotiatorItem);
                expect(leadNegotiatorUserData.text()).toEqual(leadNegotiator.user.firstName + ' ' + leadNegotiator.user.lastName);
            });
            
            it('then list of secondary negotiators should be ordered by first and last name', () =>{
                var elementNegotiatorList: ng.IAugmentedJQuery = element.find(pageObjectSelector.secondaryNegotiatorItems);                
                var expectedOrder: Business.ActivityUser[] = _.sortByOrder(secondaryNegotiators, ['user.firstName', 'user.lastName']);
                
                var isOrdered: boolean = _.every(elementNegotiatorList, (itm, ind, coll): boolean => {
                    var sourceEl:string = expectedOrder[ind].user.firstName + ' ' + expectedOrder[ind].user.lastName; 
                    
                    return itm.innerText === sourceEl;
                });            
                                    
                expect(isOrdered).toBeTruthy();
            });            
            
            describe('when secondary negotiators list is empty', () => {
                it('then no secondary negotiators id visible', () =>{
                    scope.secondaryNegotiator = null;
                    scope.$apply();
                    
                    assertValidator.assertShowElement(false, pageObjectSelector.noSecondaryNegotiators);                    
                });                
            });
        });
    });
}