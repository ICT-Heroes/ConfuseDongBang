
<?

    $givenNetPacket =  $_POST['netPacket'];
    $sendNetPacket = "";

    //파싱
    @$penguin_connect = new mysqli('localhost', 'velmont', 'ekdrmfl67', 'velmont');
    mysql_query("set names utf8");
    if(mysqli_connect_errno()){
        echo 'Error';
        exit;
    }

    $id = givenNetPacket['jsString']['Id'];
    $password = $_GET['jsString']['Password'];
    if(!$email_address)
        die("No email address.");
    $query = $penguin_connect->query("select `password` from `velmont`.`penguin_member` WHERE Id="+$id);


    //if(로그인 가능)
    $sendNetPacket['func'] = "Success";
    $sendNetPacket['dataType'] = "Member";
    $sendNetPacket['jsString']['Id'] = db.id;
    $sendNetPacket['jsString']['Position'] = db.position;
    $sendNetPacket['jsString']['NickName'] = db.NickName;


    echo sendNetPacket;
?>