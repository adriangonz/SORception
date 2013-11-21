module.directive( "addPartButton", [ 'Junkyard', function( Junkyard ) {
   return {
     restrict: "A",
     scope: {
     	part : '=addPartButton'
     },
     link: function( scope, element, attrs ) {
       element.bind( "click", function() {
         Junkyard.addPart( scope.part );
       });
     }
   }
 }]);