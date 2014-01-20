module.controller( "PedidosCtrl", [ '$scope', 'Scrap','Auth', function( $scope, Scrap, Auth ) {
	$scope.$on( 'pedidos.update', function( event ) {
		$scope.pedidos = Scrap.pedidos;
	});


	Auth.isLoggedIn();
	
	Scrap.getPedidos();
  	$scope.pedidos;

}]);
