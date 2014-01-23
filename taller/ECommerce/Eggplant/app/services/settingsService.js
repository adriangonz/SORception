module.service('SettingsService', ['$rootScope', '$http', '$timeout', function ($rootScope, $http, $timeout) {
   var service = {
     settings: {
     	"name": "",
     	"tokens": {}
     },
     userList: [],
 
     getSettings: function () {
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
     },

     postSettings: function(){
         $http({ method: 'POST', url: '/api/settings', data: '{"nombre": "Taller.Net"}' }).
          success(function (data, status, headers, config) {
              $timeout(service.getSettings, 1000);
          }).
          error(function(data, status, headers, config) {
              alert(status + " " + data);
              console.log(data);
          });
     },

     getUsers: function () {
         $http({ method: 'GET', url: '/api/account/users' }).
           success(function (data, status, headers, config) {
               service.userList = data;
               console.log("Users:");
               console.log(data);
               $rootScope.$broadcast('userList.update');
           }).
           error(function (data, status, headers, config) {
               alert(status + " | " + data);
               console.log(data);

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
         //He probado el registro externo y nada... :(
         //$http({ method: 'POST', url: '/api/account/RegisterExternal', data: user_data }).
         
         $http({ method: 'POST', url: '/api/account/register', data: user_data }).
         success(function (data, status, headers, config) {
             //service.getUsers();
             console.log("OK: " + status + " | " + data);
             $("#err-reg").html("");
             $("#suc-reg").html("Registrado! Ya puede loguearse.");
         }).
         error(function (data, status, headers, config) {
             console.log("Error: " + status + " | " + data);
             $("#suc-reg").html("");
             $("#err-reg").html("Error! Algun campo es incorrecto");
         });
     },

   }
 

   return service;
}]);

