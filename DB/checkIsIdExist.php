<?php
    header('Content-Type: application/json; charset=UTF-8');
    include('connectDB.php');
    $netPacket = json_decode(file_get_contents('php://input'), true);
    
    $clientId = $netPacket['clientId'];
    $netPacket['classType'] = "Member";
    $netPacket['echoType'] = "NotEcho";
    $netPacket['func'] = $receivedPacket['func'];
    
    $jsonString = $netPacket['jsonString'];
    $member = json_decode($jsonString, true);
    $id = $member['id'];
    $query = "SELECT * FROM penguin_member WHERE id = '$id';";
    $result = @mysqli_query($penguin_connect, $query);
      
    $num_rows = mysqli_num_rows($result);
    if($num_rows >= 0){
        if($num_rows == 0){
            die("User Id `$id` Not Exist");
        }else if($num_rows > 1){
            die("Not a unique id");
        }else{
            $row = mysqli_fetch_assoc($result);
            $member['memberSrl'] = $row['memberSrl'];
            $member['id'] = $row["id"];
            $member['password'] = $row['password'];
            $member['nickname'] = $row['nickname'];
            $member['emailAddress'] = $row['emailAddress'];
            $member['isAdmin'] = $row['isAdmin'];
            $member['regDate'] = $row['regDate'];
            $member['lastLogin'] = $row['lastLogin'];
            
            $netPacket['jsonString'] = json_encode($member);

            echo json_encode($netPacket);
        }
    }else{
        die("server access failed");
    }
    $penguin_connect->close();
?>