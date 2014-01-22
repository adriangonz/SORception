module.controller("PedidoCtrl", ['$scope', '$routeParams', 'Taller', 'Auth', function ($scope, $routeParams, Taller, Auth) {
    $scope.$on('actual_pedido.update', function (event) {
        $scope.actual_pedido = Taller.actual_pedido;
    });

    Auth.isLoggedIn();
    Taller.getPedido($routeParams.id);

}]);
