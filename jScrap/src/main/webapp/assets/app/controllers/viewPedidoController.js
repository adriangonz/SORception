module.controller( "ViewPedidoCtrl", [ '$scope','$routeParams', 'Scrap','Auth', function( $scope, $routeParams, Scrap, Auth ) {
	$scope.$on( 'actual_pedido.update', function( event ) {
		$scope.actual_pedido = Scrap.actual_pedido;
	});


	Auth.isLoggedIn();
	Scrap.getActualOffer($routeParams.id);

}]);
