module.controller("OrderCtrl", ['$scope', '$routeParams', 'Taller', 'Auth', function ($scope, $routeParams, Taller, Auth) {
    $scope.$on('actual_order.update', function (event) {
        $scope.actual_order = Taller.actual_order;
    });
    $scope.$on('pedido.update', function (event) {
        $scope.pedido = Taller.pedido;
        console.log("Pedido");
        console.log($scope.pedido);
    });

    Auth.isLoggedIn();
    Taller.getOrder($routeParams.id);
    Taller.setSolicitudPedido($routeParams.id);

}]);
