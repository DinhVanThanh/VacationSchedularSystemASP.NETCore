(function () {
    'use strict';

    angular
        .module('app.myProfile')
        .controller('MyProfileController', MyProfileController);
     

    /** @ngInject */
    function MyProfileController($scope, $log, $mdDialog, $rootScope, $http, $window, ProfileService, $mdToast, AuthenticationService) {
        
        $scope.genders = ["None", "Male", "Female", "Others"];
        $scope.minDate = new Date("1900-01-01");
        $scope.maxDate = new Date();

        AuthenticationService.GetProfileAsync().then(function (res) {
            console.log(res);
        });
    }

})();