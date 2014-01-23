module.directive( "requestTokenButton", [ 'SettingsService', function( SettingsService ) {
   return {
     restrict: "A"
     ,
     link: function( scope, element, attrs ) {
       element.bind( "click", SettingsService.postSettings);
     }
   }
 }]).directive( "postUserBtn", [ 'SettingsService', function( SettingsService ) {
   return {
     restrict: "A",
     scope: {
     	user : '=postUserBtn'
     },
     link: function( scope, element, attrs ) {
       element.bind( "click", function() {
         SettingsService.postUser( scope.user );
       });
     }
   }
 }]);
