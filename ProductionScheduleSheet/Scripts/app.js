angular.module('app.machine', ['angular-loading-bar'])
    .directive('moDateInput', function ($window) {
        return {
            require: '^ngModel',
            restrict: 'A',
            link: function (scope, elm, attrs, ctrl) {
                var moment = $window.moment;
                var dateFormat = attrs.moDateInput;
                attrs.$observe('moDateInput', function (newValue) {
                    if (dateFormat == newValue || !ctrl.$modelValue) return;
                    dateFormat = newValue;
                    ctrl.$modelValue = new Date(ctrl.$setViewValue);
                });

                ctrl.$formatters.unshift(function (modelValue) {
                    scope = scope;
                    if (!dateFormat || !modelValue) return "";
                    var retVal = moment(modelValue).format(dateFormat);
                    return retVal;
                });

                ctrl.$parsers.unshift(function (viewValue) {
                    scope = scope;
                    var date = moment(viewValue, dateFormat);
                    return (date && date.isValid() && date.year() > 1950) ? date.toDate() : "";
                });
            }
        };
    })
    //.directive('datepicker', function () {
    //    return {
    //        restrict: 'A',
    //        require: 'ngModel',
    //        link: function (scope, elem, attrs, ngModelCtrl) {
    //            var updateModel = function (dateText) {
    //                scope.$apply(function () {
    //                    ngModelCtrl.$setViewValue(dateText);
    //                });
    //            };
    //            var options = {
    //                dateFormat: 'MM/dd/yyyy',
    //                onSelect: function (dateText) {
    //                    updateModel(dateText);
    //                }
    //            };
    //            elem.datepicker(options);
    //        }
    //    };
    //})
//.directive('jqdatepicker', function ($filter) {
//    return {
//        restrict: 'A',
//        require: 'ngModel',
//        controller: function ($scope, $filter) {
//            $scope.dateValString = $filter('date')($scope.dateVal, 'mediumDate');
//        },
//        link: function (scope, element, attrs) {
//            $(element).find(".datepicker").datepicker({
//                dateFormat: 'M d, yy'
//            });
//            $(element).find(".datepicker").on('change', function () {
//                console.log("olddateVal " + scope.dateVal);
//                scope.dateVal = $(this).datepicker("getDate");
//                console.log("newDateVal " + scope.dateVal);
//                scope.$root.$broadcast("dateValueChanged", {
//                    newDate: scope.dateVal
//                });
//            });
//        }
//        //link: function (scope, element, attrs, ngModelCtrl) {
//        //    element.datepicker({
//        //        dateFormat: 'MM/dd/yyyy',
//        //        onSelect: function (date) {   
//        //            var ar=date.split("/");
//        //            date=new Date(ar[2]+"-"+ar[1]+"-"+ar[0]);
//        //            ngModelCtrl.$setViewValue(date.getTime());            
//        //            scope.$apply();
//        //        }
//        //    });
//        //    ngModelCtrl.$formatters.unshift(function(v) {
//        //        return $filter('date')(v,'dd/MM/yyyy'); 
//        //    });
//        //}
//    };
//})
.controller('machineVM', ["$scope", "$http", "$filter", function ($scope, $http, $filter) {

    $scope.servicePath = 'api/value/'
    $scope.sortType = 'JobNum';
    $scope.sortReverse = false;
    $scope.searching = '';

    $scope.Status = {
        EditForm: false,
        ClientId: '',
        UserName: '',
        AccessFailedCount: '',
    };

    $scope.Machine = {
        editRow: {},
        selected: null,
        existing: [],
    };
    $scope.BOMSequence = [];
    $scope.DocSequence = [];
    $scope.EPrintSequence = [];
    $scope.ProgramSequence = [];
    $scope.ReviewSequence = [];
    $scope.printFlags = [true, true, true, true, true, true, true, true, true, true, true, true, true, true, true];
    $scope.displayFlags = [true, true, true, true, true, true, true, true, true, true, true, true, true, true, true];

    $scope.newRow = [];
    $scope.format = "dd/MM/yyyy";


    $http.get($scope.servicePath + 'getdbxml')
        .success(function (data) {
            $scope.Machine.existing = data;
        })
        .error(function (data, status, headers, config) {
            console.log("Get User list fialed! " + data.Message + '-' + data.ExceptionMessage)
        });

    $http.get($scope.servicePath + 'BOMSequence')
    .success(function (data) {
        $scope.BOMSequence = data;
    })
    .error(function (data, status, headers, config) {
        console.log("Get User list fialed! " + data.Message + '-' + data.ExceptionMessage)
    });
    $http.get($scope.servicePath + 'DocSequence')
    .success(function (data) {
        $scope.DocSequence = data;
    })
    .error(function (data, status, headers, config) {
        console.log("Get User list fialed! " + data.Message + '-' + data.ExceptionMessage)
    });
    $http.get($scope.servicePath + 'EPrintSequence')
    .success(function (data) {
        $scope.EPrintSequence = data;
    })
    .error(function (data, status, headers, config) {
        console.log("Get User list fialed! " + data.Message + '-' + data.ExceptionMessage)
    });
    $http.get($scope.servicePath + 'ProgramSequence')
    .success(function (data) {
        $scope.ProgramSequence = data;
    })
    .error(function (data, status, headers, config) {
        console.log("Get User list fialed! " + data.Message + '-' + data.ExceptionMessage)
    });
    $http.get($scope.servicePath + 'ReviewSequence')
    .success(function (data) {
        $scope.ReviewSequence = data;
    })
    .error(function (data, status, headers, config) {
        console.log("Get User list fialed! " + data.Message + '-' + data.ExceptionMessage)
    });


    // event handlers
    $scope.Edit = function (row) {
        $scope.selectedRow = row;
        $scope.Machine.selected = $filter('filter')($scope.Machine.existing, { JobNum: row })[0];
        $scope.Machine.editRow = angular.copy($scope.Machine.selected);
        jQuery("#editDialog").dialog({
            position: { my: "center", at: "center", of: "#schedulePage" },
            width: 450,
            resizable: true,
            draggable: false,
            autoOpen: true,
            modal: true
        });
    }

    $scope.Update = function () {
        jQuery('#editDialog').dialog('close');
        $scope.Machine.selected.WarningDate = $scope.Machine.editRow.WarningDate;
        if ($scope.Machine.editRow.RevId != null) {
            $scope.Machine.selected.ReviewState = $filter('filter')($scope.ReviewSequence, { id: $scope.Machine.editRow.RevId })[0].Value;
            $scope.Machine.selected.RevId = $scope.Machine.editRow.RevId;
        }
        if ($scope.Machine.editRow.BomId != null) {
            $scope.Machine.selected.BOMState = $filter('filter')($scope.BOMSequence, { id: $scope.Machine.editRow.BomId })[0].Value;
            $scope.Machine.selected.BomId = $scope.Machine.editRow.BomId;
        }
        if ($scope.Machine.editRow.DocId != null) {
            $scope.Machine.selected.Documentation = $filter('filter')($scope.DocSequence, { id: $scope.Machine.editRow.DocId })[0].Value;
            $scope.Machine.selected.DocId = $scope.Machine.editRow.DocId;
        }
        if ($scope.Machine.editRow.EprId != null) {
            $scope.Machine.selected.EprintState = $filter('filter')($scope.EPrintSequence, { id: $scope.Machine.editRow.EprId })[0].Value;
            $scope.Machine.selected.EprId = $scope.Machine.editRow.EprId;
        }
        if ($scope.Machine.editRow.PrgId != null) {
            $scope.Machine.selected.ProgramState = $filter('filter')($scope.ProgramSequence, { id: $scope.Machine.editRow.PrgId })[0].Value;
            $scope.Machine.selected.PrgId = $scope.Machine.editRow.PrgId;
        }
        $scope.Machine.selected.ProgramOwner = $scope.Machine.editRow.ProgramOwner;
        $scope.Machine.selected.EprintOwner = $scope.Machine.editRow.EprintOwner;
        //var config = {
        //    method: 'put',
        //    url: $scope.servicePath + 'Update',
        //    data: { rec: $scope.Machine.selected }
        //};
        //$http(config)
        $http.put($scope.servicePath + 'Update', $scope.Machine.selected)
            .success(function (data) { })
            .error(function (data, status, headers, config) {
                console.log("Get User list fialed! " + data.Message + '-' + data.ExceptionMessage)
            });
        $scope.$digest();
    }
}])
