module.controller( "TallerCtrl", [ '$scope', 'Taller', function( $scope, Taller ) {
  $scope.$on( 'parts.update', function( event ) {
    $scope.scrap = Taller.parts;
  });

  $scope.scrap;
  
}]);
