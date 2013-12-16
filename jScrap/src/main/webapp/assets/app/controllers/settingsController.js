module.controller( "SettingsCtrl", [ '$scope', 'SettingsService', function( $scope, SettingsService ) {
	$scope.$on( 'settings.update', function( event ) {
		$scope.settings = SettingsService.settings;
	});

	SettingsService.getSettings();
	SettingsService.getUsers();
  	$scope.settings;

}]);
