module.controller( "ViewOfferCtrl", [ '$scope','$routeParams', 'Scrap','Auth', function( $scope, $routeParams, Scrap, Auth ) {
	$scope.$on( 'tmp_offer.update', function( event ) {
		$scope.tmp_offer = Scrap.tmp_offer;
	});
	$scope.$on( 'actual_offer.update', function( event ) {
		$scope.actual_offer = Scrap.actual_offer;
	});


	Auth.isLoggedIn();
	Scrap.getActualOffer($routeParams.id);

}]);
