(function () {
    'use strict';

    angular
        .module('app.profile')
        .factory('ProfileService', ProfileService);

    /** @ngInject */
    function ProfileService(SVCS, $cookies, $http, $q, $interval, $rootScope) {

        var baseUrl = SVCS.Profile;
        var PositionUrl = baseUrl + "/api/Position";
        var EmployeeUrl = baseUrl + "/api/Employee";
        var loadCurrentPromises = [];
        var loadCurrentProfilePromises = [];

        var service = {
              
            getAllPositions: getAllPositions,
            getAllEmployees: getAllEmployees
        };

         

        //positions
        function getAllPositions() {
            var deferer = $q.defer();
            $http.get(PositionUrl + "/PositionList").then(function (data) {
                deferer.resolve(data);
            }, function (error) {
                deferer.reject(error.data);
            });

            return deferer.promise;
        }

        //employee
        function getAllEmployees()
        {
            var deferer = $q.defer();
            $http.get(EmployeeUrl + "/EmployeeList").then(function (data) {
                deferer.resolve(data);
            }, function (error) {
                deferer.reject(error.data);
            });

            return deferer.promise;
        }

        return service;
    }
})();