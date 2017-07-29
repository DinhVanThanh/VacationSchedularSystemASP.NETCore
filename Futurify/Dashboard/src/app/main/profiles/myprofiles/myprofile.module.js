(function ()
{
    'use strict';

    angular
        .module('app.myProfile', ['app.profile'])
        .config(config);

    /** @ngInject */
    function config($stateProvider)
    { 
        $stateProvider.state('app.profile.myProfile', {
            url: '/MyProfile',
            templateUrl: 'app/main/profiles/myprofiles/views/MyProfile.html',
            controller: 'MyProfileController',
            controllerAs: 'emc'

        });
    }
})();