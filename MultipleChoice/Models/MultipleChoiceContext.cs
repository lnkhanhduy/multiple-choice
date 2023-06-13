﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MultipleChoice.Models
{
    public partial class MultipleChoiceContext : DbContext
    {
        public MultipleChoiceContext()
        {
        }

        public MultipleChoiceContext(DbContextOptions<MultipleChoiceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; } = null!;
        public virtual DbSet<Answer> Answers { get; set; } = null!;
        public virtual DbSet<Chapter> Chapters { get; set; } = null!;
        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<Exam> Exams { get; set; } = null!;
        public virtual DbSet<Grade> Grades { get; set; } = null!;
        public virtual DbSet<Learning> Learnings { get; set; } = null!;
        public virtual DbSet<Lesson> Lessons { get; set; } = null!;
        public virtual DbSet<Level> Levels { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<Result> Results { get; set; } = null!;
        public virtual DbSet<Setting> Settings { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;
        public virtual DbSet<Teacher> Teachers { get; set; } = null!;
        public virtual DbSet<Teaching> Teachings { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS; Database=MultipleChoice; User Id=sa; Password=123456;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.Username);

                entity.Property(e => e.Username)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.Property(e => e.AnswerLabel)
                    .HasMaxLength(1)
                    .IsFixedLength();

                entity.Property(e => e.IsDelete).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<Chapter>(entity =>
            {
                entity.Property(e => e.ChapterName).HasMaxLength(50);

                entity.Property(e => e.IsDelete).HasDefaultValueSql("((0))");

                entity.Property(e => e.Meta)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdSubjectNavigation)
                    .WithMany(p => p.Chapters)
                    .HasForeignKey(d => d.IdSubject)
                    .HasConstraintName("FK_Chapters_Subjects");
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.Property(e => e.ClassName).HasMaxLength(50);

                entity.Property(e => e.IsDelete).HasDefaultValueSql("((0))");

                entity.Property(e => e.Meta)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdGradeNavigation)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.IdGrade)
                    .HasConstraintName("FK_Classes_Grades");
            });

            modelBuilder.Entity<Exam>(entity =>
            {
                entity.Property(e => e.ExamDate).HasColumnType("datetime");

                entity.Property(e => e.IsDelete).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.Property(e => e.GradeName).HasMaxLength(50);

                entity.Property(e => e.IsDelete).HasDefaultValueSql("((0))");

                entity.Property(e => e.Meta)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Learning>(entity =>
            {
                entity.HasKey(e => new { e.IdStudent, e.IdClass, e.Year })
                    .HasName("PK_Learnings_1");

                entity.Property(e => e.IdStudent)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdClassNavigation)
                    .WithMany(p => p.Learnings)
                    .HasForeignKey(d => d.IdClass)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Learnings_Classes");

                entity.HasOne(d => d.IdStudentNavigation)
                    .WithMany(p => p.Learnings)
                    .HasForeignKey(d => d.IdStudent)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Learnings_Students");
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.Property(e => e.IsDelete).HasDefaultValueSql("((0))");

                entity.Property(e => e.LessonName).HasMaxLength(150);

                entity.Property(e => e.Meta)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdChapterNavigation)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.IdChapter)
                    .HasConstraintName("FK_Lessons_Chapters");
            });

            modelBuilder.Entity<Level>(entity =>
            {
                entity.Property(e => e.IsDelete).HasDefaultValueSql("((0))");

                entity.Property(e => e.LevelName).HasMaxLength(20);
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.AnswerA)
                    .HasMaxLength(250)
                    .HasColumnName("Answer_A");

                entity.Property(e => e.AnswerB)
                    .HasMaxLength(250)
                    .HasColumnName("Answer_B");

                entity.Property(e => e.AnswerC)
                    .HasMaxLength(250)
                    .HasColumnName("Answer_C");

                entity.Property(e => e.AnswerD)
                    .HasMaxLength(250)
                    .HasColumnName("Answer_D");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.EditingDate).HasColumnType("datetime");

                entity.Property(e => e.IsApprove).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsDelete).HasDefaultValueSql("((0))");

                entity.Property(e => e.Note).HasMaxLength(250);

                entity.Property(e => e.QuestionContent).HasMaxLength(250);

                entity.HasOne(d => d.IdAnswerNavigation)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.IdAnswer)
                    .HasConstraintName("FK_Questions_Answers");

                entity.HasOne(d => d.IdLessonNavigation)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.IdLesson)
                    .HasConstraintName("FK_Questions_Lessons");

                entity.HasOne(d => d.IdLevelNavigation)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.IdLevel)
                    .HasConstraintName("FK_Questions_Levels1");
            });

            modelBuilder.Entity<Result>(entity =>
            {
                entity.HasKey(e => new { e.IdStudent, e.IdExam, e.IdQuestion });

                entity.Property(e => e.IdStudent)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Answer)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.IsDelete).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdExamNavigation)
                    .WithMany(p => p.Results)
                    .HasForeignKey(d => d.IdExam)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Results_Exams");

                entity.HasOne(d => d.IdQuestionNavigation)
                    .WithMany(p => p.Results)
                    .HasForeignKey(d => d.IdQuestion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Results_Questions1");

                entity.HasOne(d => d.IdStudentNavigation)
                    .WithMany(p => p.Results)
                    .HasForeignKey(d => d.IdStudent)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Results_Members");
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.HasKey(e => e.Keyword);

                entity.Property(e => e.Keyword)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Value).HasMaxLength(250);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.IdStudent)
                    .HasName("PK_Members");

                entity.Property(e => e.IdStudent)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Address).HasMaxLength(150);

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.IsDelete).HasDefaultValueSql("((0))");

                entity.Property(e => e.Password)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.StudentName).HasMaxLength(30);
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.Property(e => e.IsDelete).HasDefaultValueSql("((0))");

                entity.Property(e => e.Meta)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SubjectName).HasMaxLength(50);
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(150);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdTeacher)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IsDelete).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsLeader).HasDefaultValueSql("((0))");

                entity.Property(e => e.Password)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.TeacherName).HasMaxLength(30);
            });

            modelBuilder.Entity<Teaching>(entity =>
            {
                entity.Property(e => e.EndingDate).HasColumnType("date");

                entity.Property(e => e.IsDelete).HasDefaultValueSql("((0))");

                entity.Property(e => e.StartingDate).HasColumnType("date");

                entity.HasOne(d => d.IdClassNavigation)
                    .WithMany(p => p.Teachings)
                    .HasForeignKey(d => d.IdClass)
                    .HasConstraintName("FK_Teachings_Classes");

                entity.HasOne(d => d.IdSubjectNavigation)
                    .WithMany(p => p.Teachings)
                    .HasForeignKey(d => d.IdSubject)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Teachings_Subjects");

                entity.HasOne(d => d.IdTeacherNavigation)
                    .WithMany(p => p.Teachings)
                    .HasForeignKey(d => d.IdTeacher)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Teachings_Teachers");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}