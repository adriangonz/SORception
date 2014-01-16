module.controller("OrderCtrl", ['$scope', '$routeParams', 'Taller', 'Auth', function ($scope, $routeParams, Taller, Auth) {
    $scope.$on('actual_order.update', function (event) {
        $scope.actual_order = Taller.actual_order;
    });
    $scope.$on('offers.update', function (event) {
        $scope.offers = Taller.offers;
    });
    
    Auth.isLoggedIn();
    Taller.getOrder($routeParams.id);
    //Taller.getOffersOf($routeParams.id);

}]);
