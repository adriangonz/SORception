module.controller( "CreateOfferCtrl", [ '$scope','$routeParams', 'Scrap','Auth', function( $scope, $routeParams, Scrap, Auth ) {
	$scope.$on( 'tmp_offer.update', function( event ) {
		$scope.tmp_offer = Scrap.tmp_offer;
	});
	$scope.$on( 'actual_order.update', function( event ) {
		$scope.actual_order = Scrap.actual_order;
	});


	Auth.isLoggedIn();
	Scrap.getActualOrder($routeParams.id);
  	$scope.tmp_offer= {"lines": []};

}]);
