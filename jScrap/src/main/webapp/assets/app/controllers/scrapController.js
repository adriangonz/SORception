module.controller( "ScrapCtrl", [ '$scope', 'Scrap', function( $scope, Scrap ) {
  $scope.$on( 'parts.update', function( event ) {
    $scope.scrap = Scrap.parts;
  });

  $scope.scrap;

}]);
