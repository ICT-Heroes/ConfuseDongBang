<!DOCTYPE html>
<html>
<head lang="ko">
    <meta charset="UTF-8">
    <title></title>
    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css">

    <!-- jQuery library -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>

    <!-- Latest compiled JavaScript -->
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>

    <!-- AngluarJS FrameWork -->
    <script src="http://ajax.googleapis.com/ajax/libs/angularjs/1.2.26/angular.min.js"></script>

    <script src="http://demos.amitavroy.com/learningci/assets/js/xml2json.js"></script>

    <script src="kcqmApp.js"></script>


    <meta name="viewport" content="width=device-width, initial-scale=1">
</head>

<body ng-app="kcqmApp" ng-controller="xmlCtrl">
<div class="root">


    <div class="page-header">
        <h1>ISO 9001 : 2005</h1>
    </div>

        <ul class="list-group">
            <li class="list-group-item">{{xmlData.root.menu[0].subject.subject_big}}</li>
        </ul>


        <ul class="pager button_ui">
            <li class="next "><a href="#">Home</a></li>
            <li class="next "><a href="#">Previous</a></li>
        </ul>

            <h3>목 록</h3>

            <div class="list-group">
                <a href="#" class="list-group-item" ng-repeat="context in xmlData.root.menu[0].context">{{context.context_subject}}</a>
            </div>

</div>

</body>
</html>