(function ()
{
    'use strict';

    angular
        .module('vacation').constant('SVCS', {
            Auth: 'http://localhost:16979',
            Profile: 'http://localhost:65236',
            Schedule: 'http://localhost:16992',
            Report: 'http://localhost:60600',
            Notification: 'http://localhost:10453'
        });;
})();
