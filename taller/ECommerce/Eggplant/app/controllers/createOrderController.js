module.controller("createOrderCtrl", ['$scope', 'Taller', 'Auth', function ($scope, Taller, Auth) {
    $scope.$on('tmp_order.update', function (event) {
        console.log(Taller.tmp_order);
        $scope.tmp_order = Taller.tmp_order;
    });

    $scope.criterios = [
    { name: 'Seleccion Manual', code: '0' },
    { name: 'El primero en llegar', code: '1' },
    { name: 'El mas barato', code: '2' },
    { name: 'El mas nuevo', code: '3' }
    ];

    $scope.tmp_order = Taller.tmp_order = { "data": [] };

    Auth.isLoggedIn();
   
}]);