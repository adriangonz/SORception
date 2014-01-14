module.service( 'SettingsService', [ '$rootScope', '$http', function( $rootScope, $http ) {
   var service = {
     settings: {
     	"name": "",
     	"tokens": {}
 		},
 
     getSettings: function () {
         $http({ method: 'GET', url: '/api/settings/token' }).
          success(function (data, status, headers, config) {
              console.log(status + " " + data);

              $http({ method: 'GET', url: '/api/settings' }).
                  success(function (data, status, headers, config) {
                      service.settings = data;
                      $rootScope.$broadcast('settings.update');
                  }).
                  error(function (data, status, headers, config) {
                      console.log(status + " " + data);
                      service.settings.name = "TallerGET";
                      $rootScope.$broadcast('settings.update');
                  });

          }).
          error(function (data, status, headers, config) {
              console.log(status + " " + data);
          });

     },

     postSettings: function(){
         $http({ method: 'POST', url: '/api/settings', data: '{"nombre": "Taller.Net"}' }).
          success(function(data, status, headers, config) {
              service.getSettings();
          }).
          error(function(data, status, headers, config) {
              alert(status+" "+data);
          });
     },

     getUsers: function () {
         $http({ method: 'GET', url: '/jScrap/api/user' }).
           success(function (data, status, headers, config) {
               service.settings.userList = data;
               $rootScope.$broadcast('settings.update');
           }).
           error(function (data, status, headers, config) {
               alert(status + " | " + data);
               service.settings.userList = [
                  {
                      "id": 1,
                      "username": "Admin",
                      "name": "su nombre",
                      "isAdmin": true,
                      "creationDate": "FECHA_1"
                  },
                  {
                      "id": 2,
                      "username": "Other",
                      "name": "su nombre",
                      "isAdmin": false,
                      "creationDate": "FECHA_2"
                  },
                  {
                      "id": 3,
                      "username": "Other2",
                      "name": "su nombre",
                      "isAdmin": false,
                      "creationDate": "FECHA_4"
                  }
               ];
               $rootScope.$broadcast('settings.update');
           });
     },

     postUser: function (user_data) {
         $http({ method: 'POST', url: '/api/account/register', data: user_data }).
         success(function (data, status, headers, config) {
             //service.getUsers();
             console.log("OK: " + status + " | " + data);
         }).
         error(function (data, status, headers, config) {
             console.log("Error: " + status + " | " + data);
         });
     },

   }
 

   return service;
}]);

