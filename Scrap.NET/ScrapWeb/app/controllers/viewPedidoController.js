module.controller( "ViewPedidoCtrl", [ '$scope','$routeParams', 'Scrap','Auth', function( $scope, $routeParams, Scrap, Auth ) {
	$scope.$on( 'actual_offer.update', function( event ) {
		$scope.actual_pedido = Scrap.actual_offer;
		Scrap.getActualOrder($scope.actual_pedido.orderId);
		
	});
	$scope.$on( 'actual_order.update', function( event ) {
		$scope.actual_order = Scrap.actual_order;
		
	});


	Auth.isLoggedIn();
	Scrap.getActualOffer($routeParams.id);

}]);
