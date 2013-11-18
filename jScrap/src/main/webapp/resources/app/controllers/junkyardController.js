module.controller( "JunkyardCtrl", [ '$scope', 'Junkyard', function( $scope, Junkyard ) {
  $scope.$on( 'parts.update', function( event ) {
    $scope.junkyard = Junkyard.parts;
  });

  $scope.junkyard;

}]);
