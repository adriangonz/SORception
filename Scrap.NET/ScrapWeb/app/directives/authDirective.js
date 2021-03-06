module.directive( "loginBtn", [ 'Auth', function( Auth ) {
   return {
     restrict: "A",
     scope: {
     	user : '=loginBtn'
     },
     link: function( scope, element, attrs ) {
       element.bind( "click", function() {
         Auth.login( scope.user );
       });
     }
   }
 }]).directive("logout", ['Auth', function (Auth) {
    return {
        restrict: "E",
        scope: {},
        link: function (scope, element, attrs) {
            element.bind("click", function () {
                Auth.logout();
            });
        }
    }
 }]).directive("config", ['Auth', function (Auth) {
     return {
         restrict: "A",
         scope: {},
         link: function (scope, element, attrs) {
             scope.$on('auth.login', function () {
                 if (Auth.getUsername() == "admin") {
                     element.html('<a href="#/config">Configuracion</a>');
                 } else {
                     element.html('');
                 }
             });
         }
     }
 }]);

