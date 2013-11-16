module.controller( "JunkyardCtrl", [ '$scope', 'Junkyard', function( $scope, Junkyard ) {
  $scope.$on( 'parts.update', function( event ) {
    console.log(Junkyard.parts);
    $scope.junkyard = Junkyard.parts;
  });
  $scope.junkyard = Junkyard.parts;

}]);
