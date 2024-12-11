# Forum_API
 Backend application for the Internet Security university project

# Internet Forum System Specification

## **Task Overview**
The task involves creating an online forum where registered users can exchange opinions on various topics. Topics are divided into categories, and the forum must support at least the following categories/rooms: **Science**, **Culture**, **Sports**, and **Music**.

The web application consists of three main parts, accessible to users based on their user group. Users are divided into three groups:
1. **Administrator**
2. **Moderator**
3. **Forum User**

---

## **System Components**

### **1. Administrator Panel**
Accessible only to the **Administrator** group, this panel allows for managing user accounts:
- Approve or deny access for registered users.
- Assign users to specific groups.
- Define permissions for registered users (e.g., adding, editing, or deleting comments in a specific category).

---

### **2. Moderator Panel**
Accessible to both **Administrator** and **Moderator** groups, this panel handles the management of forum comments:
- Approve or deny new comments.
- Optionally, adjust comments before publishing.

---

### **3. Forum Panel**
Accessible to all user groups, this panel is organized by categories/rooms:
- Displays a list of the last **20 comments** in each category.
- Each comment must show the **content**, the **username**, and the **timestamp** of publication.

---

## **User Registration and Authentication**

1. **Registration**:
   - New users register on the forum, after which the **Administrator** must approve their registration.
   - Upon approval, users receive a notification via email.

2. **Login**:
   - Users log in using **two-factor authentication**:
     - Step 1: Enter username and password.
     - Step 2: Receive a verification code via email, which must be entered to complete the login process.

3. **Access and Sessions**:
   - After successful login, users receive a **JWT token** to track their session.
   - The **JWT Controller** component issues and validates tokens.

---

## **System Architecture**
The system architecture includes the following components:

- **Access Controller**: 
  - Accepts user requests, communicates with other components, and sends responses to users.
  - Can automatically terminate a user session if the request violates defined policies.

- **Authentication Controller**:
  - Handles user authentication and authorization.
  - Issues a **JWT token** after successful login.

- **Web Application Firewall (WAF)**:
  - Scans traffic directed to the web server and blocks potentially malicious requests.
  - Ensures that requests comply with predefined rules (e.g., maximum text length, allowed sequences in input).

- **Certificate Controller**:
  - Manages issuing, monitoring, and revoking digital certificates for all system components.
  - Certificates can be managed using an external tool.

- **SIEM Component**:
  - Tracks and logs all security-sensitive requests.

---

## **Security and Communication**
- All communication between users and the system, as well as between system components, must be secured.
- The system must support **OAuth2 authentication** for login using external accounts (e.g., Google, GitHub).

---

## **Implementation Guidelines**
- Details not explicitly specified can be implemented freely.
- The system must be web-based.
- Any programming language and technologies can be used to implement the required technical details.

---

## **Project Requirements**
- The complete source code must be uploaded to Moodle before the project defense.
- Successfully defending the project task grants students the right to attend the oral exam.
- The project task is valid from the first session of the January-February 2024 exam period until the next project task is announced.

--- 
