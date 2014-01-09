module.controller("OrderCtrl", ['$scope', 'Taller', 'Auth', function ($scope, Taller, Auth) {
    $scope.$on('actual_order.update', function (event) {
        $scope.actual_order = Taller.actual_order;
    });
    
    Auth.isLoggedIn();
    Taller.getOrder($routeParams.id);

}]);
