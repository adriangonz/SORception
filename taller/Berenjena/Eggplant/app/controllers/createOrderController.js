module.controller("createOrderCtrl", ['$scope', 'Taller', 'Auth', function ($scope, Taller, Auth) {
    $scope.$on('tmp_order.update', function (event) {
        console.log(Taller.tmp_order);
        $scope.tmp_order = Taller.tmp_order;
        $scope.tmp_line = { "criterio": $scope.criterios[0] };
    });

    Auth.isLoggedIn();

    $scope.criterios = Taller.criterios;

    $scope.tmp_order = Taller.tmp_order = { "data": [] };
    $scope.tmp_line = {"criterio": $scope.criterios[0]};
    
}]);