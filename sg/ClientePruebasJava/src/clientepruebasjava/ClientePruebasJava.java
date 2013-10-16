/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package clientepruebasjava;

import java.io.BufferedReader;
import java.io.InputStreamReader;

/**
 *
 * @author Ruben
 */
public class ClientePruebasJava {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        try {
            Integer eid;
            BufferedReader br = new BufferedReader(new InputStreamReader(System.in));

            System.out.println("Enter number");
            eid= Integer.parseInt(br.readLine());

            System.out.println(addNewEmployee(eid));
        } catch (Exception e1) {
            System.out.println(e1.getMessage());
        }
    }

    private static String addNewEmployee(java.lang.Integer n) {
        org.tempuri.Service1 service = new org.tempuri.Service1();
        org.tempuri.IService1 port = service.getBasicHttpBindingIService1();
        return port.getData(n);
    }

}
