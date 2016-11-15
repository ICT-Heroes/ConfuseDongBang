<?php
    include('connectDB.php');
    $receivedPacket = $_POST['netPacket'];
    $clientId = $_POST['netPacket']['clientId'];
    $id = $_POST['netPacket']['jsonString']['id'];
    echo $id;
    echo $receivedPacket;
   
    $netPacket = array();
    $netPacket['classType'] = "Member";
    $netPacket['echoType'] = "NotEcho";
    $netPacket['clientId'] = $receivedPacket['clientId'];
    $netPacket['func'] = $receivedPacket['func'];


    $query = "SELECT * from  `velmont`.`penguin_member` WHERE id='$id')";
    $result = mysqli_query($penguin_connect, $query);
      
   $num_rows = mysqli_num_rows($result);
    if($num_rows > 0){
        echo "ban teemo";
        if($num_rows == 0){
            die("User Id Not Exist");
        }else if($num_rows > 1){
            die("Not a unique id");
        }else{
            
            $row = mysqli_fetch_assoc($result);
            $memberData = array();
            $memberData['memberSrl'] = $row["memberSrl"];
            $memberData['id'] = $row["id"];
            $memberData['password'] = $row['password'];
            $memberData['nickname'] = $row['nickname'];
            $memberData['emailAddress'] = $row['emailAddress'];
            $memberData['isAdmin'] = $row['isAdmin'];
            $memberData['regDate'] = $row['regDate'];
            $memberData['lastLogin'] = $row['lastLogin'];
            
            echo "ban echo";
            echo $json_encode($memberData);
            
        }
    }else{
        die("server access failed");
    }
    
    $penguin_connect->close();
?>