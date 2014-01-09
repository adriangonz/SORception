module.controller("SettingsCtrl", ['$scope', 'SettingsService', 'Auth', function ($scope, SettingsService, Auth) {
	$scope.$on( 'settings.update', function( event ) {
		$scope.settings = SettingsService.settings;
	});

	Auth.isLoggedIn();

	SettingsService.getSettings();
  	$scope.settings;

}]);
