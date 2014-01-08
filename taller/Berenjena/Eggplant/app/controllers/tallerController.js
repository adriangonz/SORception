module.controller("TallerCtrl", ['$scope', 'Taller', 'Auth', function ($scope, Taller, Auth) {
  $scope.$on( 'parts.update', function( event ) {
    $scope.scrap = Taller.parts;
  });

  $scope.scrap;
  
}]);
