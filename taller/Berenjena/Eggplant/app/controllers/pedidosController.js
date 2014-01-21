module.controller("PedidosCtrl", ['$scope', 'Taller', 'Auth', function ($scope, Taller, Auth) {
    $scope.$on('pedidos.update', function (event) {
        $scope.pedidos = Taller.pedidos;
    });


    Auth.isLoggedIn();
    $scope.pedidos = [];
    Taller.getPedidos();



}]);
