<?php
    include('connectDB.php');
    $receivedPacket = $_POST['netPacket'];
    $clientId = $_POST['netPacket']['clientId'];

    $netPacket = array();
    $netPacket['classType'] = "Member";
    $netPacket['echoType'] = "NotEcho";
    $netPacket['clientId'] = $receivedPacket['clientId'];
    $netPacket['func'] = $receivedPacket['func'];


    $query = "SELECT * from  `velmont`.`penguin_member` WHERE id='+$id+')";
    $result = mysqli_query($penguin_connect, $sql);
   
   $num_rows = mysqli_num_rows($query);
    if($num_rows > 0){
        echo 'success';
        
        if($num_rows == 0){
            die("User Id Not Exist");
        }else if($num_rows > 1)
        {
            die("Not a unique id");
        }else{
            echo 'get id';
            
            $row = mysqli_fetch_assoc($result);
            $memberData = array();
            $memberData['']
            
            
        }
    }else{
        die("server access failed");
    }
    
    $penguin_connect->close();
?>