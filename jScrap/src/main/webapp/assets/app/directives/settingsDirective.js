module.directive( "requestTokenButton", [ 'SettingsService', function( SettingsService ) {
   return {
     restrict: "A"
     ,
     link: function( scope, element, attrs ) {
       element.bind( "click", SettingsService.postSettings);
     }
   }
 }])
.directive( "checkTokenStatusButton", ['SettingsService', function(SettingsService) {
  return {
    link: function(scope, element) {
      element.bind( "click", SettingsService.getSettings);
    }
  }
}])
.directive( "postUserBtn", [ 'SettingsService', function( SettingsService ) {
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
