module.controller("TallerCtrl", ['$scope', 'Taller', 'Auth', function ($scope, Taller, Auth) {
    $scope.$on('orders.update', function (event) {
        console.log(Taller.orders);
        $scope.orders = Taller.orders;
    });

    Auth.isLoggedIn();
    $scope.orders = [];
    Taller.getOrders();
  
}]);
