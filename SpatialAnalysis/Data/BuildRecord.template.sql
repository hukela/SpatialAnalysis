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

CREATE TABLE `record_[id]` (
  `id` bigint unsigned NOT NULL AUTO_INCREMENT COMMENT '文件id',
  `parent_id` bigint unsigned DEFAULT NULL COMMENT '上一级的id',
  `parent_record` int unsigned DEFAULT NULL COMMENT '所对应的表id',
  `fall_name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '文件全名',
  `type` bit(1) NOT NULL COMMENT '是否是文件夹',
  `plies` int unsigned DEFAULT NULL COMMENT '上面有多少个文件夹',
  `size` varchar(14) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '大小(字节)',
  `space_usage` varchar(14) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '占用空间(字节)',
  `create_time` datetime DEFAULT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `visit_time` datetime DEFAULT NULL COMMENT '访问时间',
  `owner` varchar(30) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '所有者',
  `exception_code` tinyint DEFAULT '0' COMMENT '异常码',
  `file_count` bigint unsigned DEFAULT '0' COMMENT '文档类文件个数',
  `picture_count` bigint unsigned DEFAULT '0' COMMENT '图片类文件个数',
  `video_count` bigint unsigned DEFAULT '0' COMMENT '视频类文件个数',
  `project_count` bigint unsigned DEFAULT '0' COMMENT '工程文件类文件个数',
  `dll_count` bigint unsigned DEFAULT '0' COMMENT '可执行类文件个数',
  `txt_count` bigint unsigned DEFAULT '0' COMMENT '文本文件个数',
  `data_count` bigint unsigned DEFAULT '0' COMMENT '压缩包类和数据类',
  `null_count` bigint unsigned DEFAULT '0' COMMENT '无后缀名文件个数',
  `other_count` bigint unsigned DEFAULT '0' COMMENT '其它文件个数',
  `create_variance` double unsigned DEFAULT NULL COMMENT '下一级文件或文件夹创建时间的方差',
  `create_average` datetime DEFAULT NULL COMMENT '下一级文件或文件夹的平均创建时间',
  PRIMARY KEY (`id`),
  KEY `prient` (`parent_id`),
  CONSTRAINT `for_prient_[id]` FOREIGN KEY (`parent_id`) REFERENCES `record_[id]` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
