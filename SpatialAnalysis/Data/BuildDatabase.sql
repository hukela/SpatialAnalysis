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
CREATE DATABASE /*!32312 IF NOT EXISTS*/`spatial_analysis` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;

USE `spatial_analysis`;

/*Table structure for table `dir_index` */

DROP TABLE IF EXISTS `dir_index`;

CREATE TABLE `dir_index` (
  `path` varchar(255) COLLATE utf8mb4_general_ci NOT NULL COMMENT '路径',
  `incident_id` int unsigned NOT NULL COMMENT '事件id',
  `targect_id` bigint unsigned NOT NULL COMMENT '对应节点id',
  PRIMARY KEY (`path`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Table structure for table `dir_tag` */

DROP TABLE IF EXISTS `dir_tag`;

CREATE TABLE `dir_tag` (
  `id` int unsigned NOT NULL AUTO_INCREMENT COMMENT 'id',
  `tag_id` int unsigned NOT NULL COMMENT '对应的标签id',
  `path` varchar(255) COLLATE utf8mb4_general_ci NOT NULL COMMENT '标签所标注的目录',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Table structure for table `incident` */

DROP TABLE IF EXISTS `incident`;

CREATE TABLE `incident` (
  `id` int unsigned NOT NULL AUTO_INCREMENT COMMENT '事件id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `title` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '事件标题',
  `explain` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL COMMENT '事件描述',
  `incident_state` tinyint DEFAULT NULL COMMENT '状态，0代表正常',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Table structure for table `tag` */

DROP TABLE IF EXISTS `tag`;

CREATE TABLE `tag` (
  `id` int unsigned NOT NULL AUTO_INCREMENT COMMENT 'id',
  `parent_id` int unsigned DEFAULT NULL COMMENT '上一级的id',
  `name` varchar(30) COLLATE utf8mb4_general_ci NOT NULL COMMENT '标签名称',
  `color` char(7) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '标签颜色',
  PRIMARY KEY (`id`),
  KEY `for_tag` (`parent_id`),
  CONSTRAINT `for_tag` FOREIGN KEY (`parent_id`) REFERENCES `tag` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Data for the table `tag` */

insert  into `tag`(`id`,`parent_id`,`name`,`color`) values (1,NULL,'系统文件','#FFFFFF'),(2,NULL,'用户文件','#FFFFFF'),(3,NULL,'软件','#FFFFFF');

/*Table structure for table `tag_view` */

DROP TABLE IF EXISTS `tag_view`;

/*!50001 DROP VIEW IF EXISTS `tag_view` */;
/*!50001 DROP TABLE IF EXISTS `tag_view` */;

/*!50001 CREATE TABLE  `tag_view`(
 `id` int unsigned ,
 `name` varchar(10) ,
 `path` varchar(255) 
)*/;

/*View structure for view tag_view */

/*!50001 DROP TABLE IF EXISTS `tag_view` */;
/*!50001 DROP VIEW IF EXISTS `tag_view` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `tag_view` AS select `tag`.`id` AS `id`,`tag`.`name` AS `name`,`dir_tag`.`path` AS `path` from (`tag` join `dir_tag` on((`tag`.`id` = `dir_tag`.`tag_id`))) */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
