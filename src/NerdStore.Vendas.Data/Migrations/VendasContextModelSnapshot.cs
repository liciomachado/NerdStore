﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NerdStore.Vendas.Data;

#nullable disable

namespace NerdStore.Vendas.Data.Migrations
{
    [DbContext(typeof(VendasContext))]
    partial class VendasContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("NerdStore.Vendas.Domain.Pedido", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ClienteId")
                        .HasColumnType("char(36)");

                    b.Property<int>("Codigo")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("Desconto")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("PedidoStatus")
                        .HasColumnType("int");

                    b.Property<decimal>("ValorTotal")
                        .HasColumnType("decimal(65,30)");

                    b.Property<Guid?>("VoucherId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("VoucherUtilizado")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("VoucherId");

                    b.ToTable("Pedidos");
                });

            modelBuilder.Entity("NerdStore.Vendas.Domain.PedidoItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("PedidoId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ProdutoId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ProdutoNome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int");

                    b.Property<decimal>("ValorUnitario")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.HasIndex("PedidoId");

                    b.ToTable("PedidoItems");
                });

            modelBuilder.Entity("NerdStore.Vendas.Domain.Voucher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Ativo")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Codigo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DataUtilizacao")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DataValidade")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal?>("Percentual")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int");

                    b.Property<int>("TipoDescontoVoucher")
                        .HasColumnType("int");

                    b.Property<bool>("Utilizado")
                        .HasColumnType("tinyint(1)");

                    b.Property<decimal?>("ValorDesconto")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.ToTable("Vouchers");
                });

            modelBuilder.Entity("NerdStore.Vendas.Domain.Pedido", b =>
                {
                    b.HasOne("NerdStore.Vendas.Domain.Voucher", "Voucher")
                        .WithMany("Pedidos")
                        .HasForeignKey("VoucherId");

                    b.Navigation("Voucher");
                });

            modelBuilder.Entity("NerdStore.Vendas.Domain.PedidoItem", b =>
                {
                    b.HasOne("NerdStore.Vendas.Domain.Pedido", "Pedido")
                        .WithMany("PedidoItems")
                        .HasForeignKey("PedidoId")
                        .IsRequired();

                    b.Navigation("Pedido");
                });

            modelBuilder.Entity("NerdStore.Vendas.Domain.Pedido", b =>
                {
                    b.Navigation("PedidoItems");
                });

            modelBuilder.Entity("NerdStore.Vendas.Domain.Voucher", b =>
                {
                    b.Navigation("Pedidos");
                });
#pragma warning restore 612, 618
        }
    }
}
