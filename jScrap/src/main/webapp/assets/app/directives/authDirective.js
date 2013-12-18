module.directive( "requestTokenButton", [ 'Auth', function( Auth ) {
   return {
     restrict: "A"
     ,
     link: function( scope, element, attrs ) {
       element.bind( "click", SettingsService.postSettings);
     }
   }
 }]).directive( "loginBtn", [ 'SettingsService', function( SettingsService ) {
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
 }]);
