module.controller( "OffersCtrl", [ '$scope', 'Scrap','Auth', function( $scope, Scrap, Auth ) {
	$scope.$on( 'offers.update', function( event ) {
		$scope.offers = Scrap.offers;
	});


	Auth.isLoggedIn();
	
	Scrap.getOffers();
  	$scope.orders;

}]);
