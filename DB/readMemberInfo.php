<?php
    header('Content-Type: application/json; charset=UTF-8');
    include('connectDB.php');
    $netPacket = json_decode(file_get_contents('php://input'), true);
    
    $netPacket['classType'] = "Member";
    $netPacket['echoType'] = "NotEcho";
    $netPacket['func'] = $receivedPacket['func'];
    
    $jsonString = $netPacket['jsonString'];
    $member = json_decode($jsonString, true);
    $id = $member['id'];
    $query = "SELECT * FROM penguin_member_info WHERE id = '$id';";
    $result = @mysqli_query($penguin_connect, $query);
      
    $num_rows = mysqli_num_rows($result);
    if($num_rows >= 0){
        if($num_rows == 0){
             $row = mysqli_fetch_assoc($result);
            $member['id'] = "";
            $member['memberSrl'] = $row['memberSrl'];
            $member['password'] = $row['password'];
            $member['emailAddress'] = $row['emailAddress'];
            $member['nickname'] = $row['nickname'];
            $member['isAdmin'] = $row['isAdmin'];
            $member['regDate'] = $row['regDate'];
            $member['lastLogin'] = $row['lastLogin'];
            
            $netPacket['jsonString'] = json_encode($member);

            echo json_encode($netPacket);
        }else if($num_rows > 1){
            die("Not a unique id");
        }else{
            $row = mysqli_fetch_assoc($result);
            $member['id'] = $row["id"];
            $member['memberSrl'] = $row['memberSrl'];
            $member['password'] = $row['password'];
            $member['emailAddress'] = $row['emailAddress'];
            $member['nickname'] = $row['nickname'];
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