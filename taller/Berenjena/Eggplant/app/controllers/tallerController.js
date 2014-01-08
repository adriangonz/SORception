module.controller("TallerCtrl", ['$scope', 'Taller', 'Auth', function ($scope, Taller, Auth) {
    $scope.$on('orders.update', function (event) {
        $scope.orders = Taller.orders;
    });
    $scope.$on('tmp_order.update', function (event) {
        alert("update");
        console.log(Taller.tmp_order);
        $scope.tmp_order = Taller.tmp_order;
    });

    Auth.isLoggedIn();
    $scope.orders = [];
    $scope.tmp_order = [];
  
}]);
