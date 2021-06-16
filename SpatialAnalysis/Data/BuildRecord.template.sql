/*
SQLyog Professional v12.09 (64 bit)
MySQL - 8.0.22 : Database - spatial_analysis
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
USE `spatial_analysis`;

/*Table structure for table `record` */

DROP TABLE IF EXISTS `record_[id]`;

CREATE TABLE `record_[id]` (
  `id` bigint unsigned NOT NULL AUTO_INCREMENT COMMENT '文件id',
  `parent_id` bigint unsigned NOT NULL COMMENT '上一级的id',
  [isFirstRecord]
  `incident_id` int unsigned DEFAULT '0' COMMENT '所对应的表id',
  `target_id` bigint unsigned DEFAULT '0' COMMENT '指向对应表中节点的id',
  [/isFirstRecord]
  `path` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '路径',
  `plies` int unsigned DEFAULT NULL COMMENT '上面有多少个文件夹',
  `size` varchar(14) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '大小(字节)',
  `space_usage` varchar(14) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '占用空间(字节)',
  `create_time` datetime DEFAULT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `visit_time` datetime DEFAULT NULL COMMENT '访问时间',
  `owner` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '所有者',
  `exception_code` int DEFAULT '0' COMMENT '异常码',
  `file_count` int unsigned DEFAULT '0' COMMENT '文件总数',
  `dir_count` int unsigned DEFAULT '0' COMMENT '文件夹总数',
  PRIMARY KEY (`id`),
  UNIQUE KEY `path` (`path`),
  KEY `prient` (`parent_id`),
  CONSTRAINT `for_prient_[id]` FOREIGN KEY (`parent_id`) REFERENCES `record_[id]` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Data for the table `record` */

insert  into `record_[id]`(`id`,`parent_id`,[isFirstRecord]`incident_id`,`target_id`,[/isFirstRecord]`path`,`plies`,`size`,`space_usage`,`create_time`,`modify_time`,`visit_time`,`owner`,`exception_code`,`file_count`,`dir_count`) values  (0,0,[isFirstRecord]0,0,[/isFirstRecord]'\\\\.\\',0,'0','0','1601-01-01 00:00:00','1601-01-01 00:00:00','1601-01-01 00:00:00',NULL,0,0,0);


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
