module.controller("TallerCtrl", ['$scope', 'Taller', 'Auth', '$timeout', function ($scope, Taller, Auth, $timeout) {
    $scope.$on('orders.update', function (event) {
        $scope.orders = Taller.orders;
        $timeout(Taller.getOrders, 3000);
    });


    Auth.isLoggedIn();
    $scope.orders = [];
    Taller.getOrders();

   
  
}]);
