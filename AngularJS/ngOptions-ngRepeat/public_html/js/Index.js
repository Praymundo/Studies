var app = angular.module("MainApp", []);
app.controller("Controller1", function ($scope) {

    $scope.States = [
        {
            Id: "SP",
            Description: "Sao Paulo"
        },
        {
            Id: "RJ",
            Description: "Rio de Janeiro"
        }
    ];
    
    $scope.Cities = [];
    $scope.SelectedCity = "";

    $scope.SelectState = function () {
        if ($scope.SelectedState === "SP") {
            $scope.Cities = [
                {
                    Id: "C1",
                    Description: "City 1 - SP"
                },
                {
                    Id: "C2",
                    Description: "City 2 - SP"
                },
                {
                    Id: "C3",
                    Description: "City 3 - SP"
                }
            ];
        } else if ($scope.SelectedState === "RJ") {
            $scope.Cities = [
                {
                    Id: "C10",
                    Description: "City 10 - RJ"
                },
                {
                    Id: "C11",
                    Description: "City 11 - RJ"
                },
                {
                    Id: "C12",
                    Description: "City 12 - RJ"
                },
                {
                    Id: "C13",
                    Description: "City 13 - RJ"
                }
            ];
        }
        
        // Just select some city
        var newCity = Math.floor((Math.random() * $scope.Cities.length));
        $scope.SelectedCity = $scope.Cities[newCity].Id;
    };
    
    $scope.LoadFromDatabase = function () {
        var newState = Math.floor((Math.random() * $scope.States.length));
        $scope.SelectedState = $scope.States[newState].Id;
        $scope.SelectState();
        $scope.ChangeCity();
    };
    
    $scope.ChangeCity = function(){
        var newCity = Math.floor((Math.random() * $scope.Cities.length));
        $scope.SelectedCity = $scope.Cities[newCity].Id;
    };
});


