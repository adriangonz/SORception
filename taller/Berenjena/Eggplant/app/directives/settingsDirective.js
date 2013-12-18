module.directive( "requestTokenButton", [ 'SettingsService', function( SettingsService ) {
   return {
     restrict: "A"
     ,
     link: function( scope, element, attrs ) {
       element.bind( "click", SettingsService.postSettings);
     }
   }
 }]);
