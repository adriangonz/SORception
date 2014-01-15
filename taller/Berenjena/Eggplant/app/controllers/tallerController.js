module.controller("TallerCtrl", ['$scope', 'Taller', 'Auth', '$timeout', function ($scope, Taller, Auth, $timeout) {
    $scope.$on('orders.update', function (event) {
        $scope.orders = Taller.orders;
    });

    Auth.isLoggedIn();
    $scope.orders = [];
    Taller.getOrders();

    $timeout(Taller.getOrders,1000);
  
}]);
